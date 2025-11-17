using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFaceDirection : ObjectAbstractBehavior {

	public enum FaceDirections{
		Right = 1,
		Left = -1
	}
	public int animStateTurn;
	public FaceDirections direction;
	public bool isManual = false;
	public bool turning;

	private Rigidbody2D rigid2d;
	private bool isAnimationPlayed;
	private FaceDirections prevDirection;

	// Use this for initialization
	void Start () {
		//if (transform.localScale.x > 0) {
		//	direction = Directions.Left;
		//} else if (transform.localScale.x < 0) {
		//	direction = Directions.Right;
		//}
		rigid2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		if (!isManual) {
			if (rigid2d) {
				if (rigid2d.linearVelocity.normalized.x < 0f) {
					direction = FaceDirections.Left;
				} else if (rigid2d.linearVelocity.normalized.x > 0f) {
					direction = FaceDirections.Right;
				}
			}
		}

		if (direction == FaceDirections.Left && direction != prevDirection) {			
			transform.localScale = new Vector3 (-Mathf.Abs ((float)direction * transform.localScale.x), transform.localScale.y, transform.localScale.z);
			prevDirection = direction;
			isAnimationPlayed = false;
			//Debug.Log (direction + "   " + prevDirection);

		} else if (direction == FaceDirections.Right && direction != prevDirection) {			
			transform.localScale = new Vector3 (Mathf.Abs ((float)direction * transform.localScale.x), transform.localScale.y, transform.localScale.z);
			prevDirection = direction;
			isAnimationPlayed = false;
		}
		//Debug.Log (isTurnAnimationPlayed);
		if (!isAnimationPlayed && animStateTurn != 0) {
			this.ToggleScripts (false, this.name);
			isAnimationPlayed = true;
			turning = true;
		}
	}

	void SetAnimationState(){
		turning = false;

		this.ToggleScripts (true, this.name);
	}
}