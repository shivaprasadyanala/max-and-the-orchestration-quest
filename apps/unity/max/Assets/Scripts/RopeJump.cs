using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class RopeJump : StickToRope {

	public Vector2 jumpVelocity = new Vector2(2, 5);
	public bool jumpingOffRope;
	public float resetDelay = .2f;

	private float timeElapsed = 0;

	void start(){

	}

	// Update is called once per frame
	protected override void Update () {

		if (collisionState.onRope && !collisionState.standing) {

			//var buttonPressed = inputState.GetButtonValue(inputButtons[0]);
			var jumpButtonPressed = CrossPlatformInputManager.GetButtonDown("Jump");
			var rightButtonPressed = inputState.GetButtonValue (inputButtons [1]);
			//var swingRightButtonHoldTime = inputState.GetButtonHoldTime (inputButtons [2])
			var leftButtonPressed = inputState.GetButtonValue (inputButtons [2]);

			if(jumpButtonPressed && !jumpingOffRope){
				ToggleScripts (false, this.GetType().ToString());
				jumpingOffRope = true;

				JumpOfTheRope ();

				//inputState.direction = inputState.direction == Directions.Right ? Directions.Left : Directions.Right;
				faceDir.enabled = true;
				//inputState.isManual = true;
				if (rightButtonPressed) {					
					inputState.direction = Directions.Right;

				} else if (leftButtonPressed) {
					inputState.direction = Directions.Left;
				}
				playerRigidBody2d.linearVelocity = new Vector2 (jumpVelocity.x * (float)inputState.direction, jumpVelocity.y);
			}

		}

		if (jumpingOffRope) {
			timeElapsed += Time.deltaTime;

			if(timeElapsed > resetDelay){
				ToggleScripts(true, this.GetType().ToString());

				ToggleCollidersTrigger (GetComponent<CircleCollider2D> (), false, this.GetType ().ToString ());
				ToggleCollidersTrigger (GetComponent<BoxCollider2D> (), false, this.GetType ().ToString ());

				//playerCircleCollider2d.isTrigger = false;
				//playerBoxCollider2d.isTrigger = false;

				jumpingOffRope = false;
				timeElapsed = 0;
			}
		}

	}

	void JumpOfTheRope(){
		dist2d.enabled = false;
		playerRigidBody2d.gravityScale = defaultGravityScale;
		playerRigidBody2d.linearDamping = defaultDrag;

		ToggleCollidersTrigger (GetComponent<CircleCollider2D> (), true, this.GetType ().ToString ());
		ToggleCollidersTrigger (GetComponent<BoxCollider2D> (), true, this.GetType ().ToString ());

		//playerCircleCollider2d.isTrigger = true;
		//playerBoxCollider2d.isTrigger = true;

		//Debug.Log ("dis2here");
	}
		
}
