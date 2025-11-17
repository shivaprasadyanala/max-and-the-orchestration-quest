using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectJump : ObjectAbstractBehavior {

	public Vector2 jumpPower;
	public int animStateJump_start;
	public int animStateJump_inTheAir;
	public int animStateJump_end;

	public bool jumping;
	public bool isInAir = false;
	public bool isJumpStartKneeing;
	public bool isJumpEndKneeing;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (jumping && !isInAir && (objectCollisionState.onGround || objectCollisionState.nearCollidedObject) && !isJumpStartKneeing && !isJumpEndKneeing) {
			isJumpStartKneeing = true;
		}
			
		if (!objectCollisionState.onGround && !objectCollisionState.nearCollidedObject) {
			isInAir = true;
			isJumpStartKneeing = false;
			isJumpEndKneeing = false;

		} else if ((objectCollisionState.onGround || objectCollisionState.nearCollidedObject) && isInAir && !isJumpEndKneeing) {
			isInAir = false;
			isJumpEndKneeing = true;
			isJumpStartKneeing = false;
		}
	}

	void StartJump(){
		objectRigidbody2D.AddForce (new Vector2(jumpPower.x * (float)objectFaceDirectionBehavior.direction, jumpPower.y));
		isJumpStartKneeing = false;

		ToggleScripts (false, this.name);
	}

	void EndJump(){
		isJumpEndKneeing = false;
		jumping = false;

		ToggleScripts (true, this.name);
	}
}
