using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Push : AbstractBehavior {

	public float speed = 1f;
	public bool onPushBlockDetected;
	public bool onPullBlockDetected;
	public bool onGrabBlockDetected;
	public bool onStandBesideBlockDetected;
	private LongJump jumpBehavior;
	private Duck duckBehavior;
	//private Equip equipBehavior;
	private FaceDirection faceDirVehavior;
	//private LadderClimbing ladderClimbingBehavior;

	// Use this for initialization
	void Start () {
		jumpBehavior = GetComponent<LongJump> ();
		duckBehavior = GetComponent<Duck> ();
		faceDirVehavior =  GetComponent<FaceDirection> ();
		//equipBehavior = GetComponent <Equip> ();
		//ladderClimbingBehavior = GetComponent<LadderClimbing> ();
	}
	
	// Update is called once per frame
	void Update () {
		var right = inputState.GetButtonValue (inputButtons [0]);
		var left = inputState.GetButtonValue (inputButtons [1]);
		//var run = inputState.GetButtonValue (inputButtons [2]);
		//var pull = inputState.GetButtonValue (inputButtons [2]);
		var pullButtonPressed = CrossPlatformInputManager.GetButton("Interaction");

		//preventing #1 : if pushblock forced by something
		if (collisionState.onPushBlock) {
			var pushBlock = collisionState.onPushBlock.GetComponent<PushBlock> ();
			if (pushBlock) {
				if (pushBlock.forcedBySomething) {
					OffPushing ();
					return;
				}
			}
		}

		//preventing #2
		if ((!collisionState.standing && !collisionState.onRope /*&& !collisionState.onLadder*/ && !duckBehavior.ducking && dist2d.enabled) || (!pullButtonPressed && dist2d.enabled && !collisionState.onRope)) {
			//Debug.Log ("herereree2222222222");
			OffPushing ();
		}

		//preventing #3 : if player jumps before pushing
		//if ((jumpBehavior.isLongJumping || jumpBehavior.isShortJumping) && !faceDirVehavior.enabled && !dist2d.enabled && !collisionState.onRope /*&& !collisionState.onLadder*/ && !collisionState.onLedgeGrabbing) {
		if ((jumpBehavior.isLongJumping || jumpBehavior.isShortJumping) && onStandBesideBlockDetected){
			onStandBesideBlockDetected = false;
			//Debug.Log ("herereree3333333");
			ToggleScripts (true, this.GetType().ToString());
		}

		//preventing #4 : if player approuching (hole or the pushblock fall down) while pushing, pushing will be canceled
		//if ((onPushBlockDetected || onGrabBlockDetected) && (!collisionState.bottomFrontObject || collisionState.bottomFrontObject == collisionState.onPushBlock)) {
		if (!collisionState.bottomFrontObject || collisionState.bottomFrontObject == collisionState.onPushBlock) {
			//Debug.Log ("here...4444");
			OffPushing ();
			return;
		}

		//preventing #5 : if onPushBlockDetected = true and player's face is away from pushblock (it rearly and suddenlly happens)
		if ((onPullBlockDetected || onGrabBlockDetected || onPushBlockDetected) && !collisionState.onPushBlock) {
			//Debug.Log ("here...555555");
			OffPushing ();
		}

		//pushing with "push-button"
		if ((right && inputState.direction == Directions.Right && collisionState.onPushBlock && collisionState.standing && pullButtonPressed && !duckBehavior.ducking && collisionState.bottomFrontObject != collisionState.onPushBlock) ||
			(left && inputState.direction == Directions.Left && collisionState.onPushBlock && collisionState.standing && pullButtonPressed && !duckBehavior.ducking && collisionState.bottomFrontObject != collisionState.onPushBlock)) {
//			if (equit.currentItem == 1) { //turn of the flashlight
//				equit.currentItem = 0;
//			}

			onPullBlockDetected = false;
			onGrabBlockDetected = false;

			if (!onPushBlockDetected) {				
				jumpBehavior.enabled = false;
				duckBehavior.enabled = false;
				//ladderClimbingBehavior.enabled = false;
				ToggleScripts (false, this.GetType().ToString());
				onPushBlockDetected = true;

				if (!dist2d.enabled) {
					dist2d.enabled = true;
					dist2d.connectedBody = collisionState.onPushBlock.gameObject.GetComponent<Rigidbody2D> ();
					dist2d.autoConfigureDistance = true;
					//dist2d.distance = .4f;
					//dist2d.distance = collisionState.onPushBlock.gameObject.GetComponent<BoxCollider2D> ().bounds.size.x/2f;
					dist2d.enableCollision = false;
				}
			}
			if (onPushBlockDetected) {
				float velX = 0f;
				velX = speed * (right ? Mathf.Abs (transform.localScale.x) : -Mathf.Abs (transform.localScale.x));
				if (collisionState.onEnvironmentElement && collisionState.onEnvironmentElement.tag.ToLower ().Contains ("liquid")) {					
						velX /= 2f;
				}
				playerRigidBody2d.linearVelocity = new Vector2 (velX, playerRigidBody2d.linearVelocity.y);
			}

		//pushing without "push-button"
		} else if (	(right && inputState.direction == Directions.Right && collisionState.onPushBlock && collisionState.standing && collisionState.onPushBlock != collisionState.standing && !pullButtonPressed && !duckBehavior.ducking) || //collisionState.onPushBlock != collisionState.standing menas player doesnt stand on the push block
					(left && inputState.direction == Directions.Left && collisionState.onPushBlock && collisionState.standing && collisionState.onPushBlock != collisionState.standing && !pullButtonPressed && !duckBehavior.ducking)) {
			/*if (!collisionState.onRope && !collisionState.onLadder) {
				jumpBehavior.enabled = true;
				duckBehavior.enabled = true;
				ToggleScripts (true, this.GetType().ToString());
				onPushBlockDetected = false;
				onPullBlockDetected = false;
				onGrabBlockDetected = false;
			}*/
			OffPushing ();
			onStandBesideBlockDetected = true;
			//Debug.Log ("just stand left side");
			if (inputState.absVelX != 0) {
				playerRigidBody2d.linearVelocity = Vector2.zero;
			}
			if (faceDirVehavior.enabled) {
				//ToggleScripts (false, this.GetType().ToString());
			}


		} else if (	(left && inputState.direction == Directions.Right && collisionState.onPushBlock && collisionState.standing && !pullButtonPressed && !duckBehavior.ducking) ||
					(right && inputState.direction == Directions.Left && collisionState.onPushBlock && collisionState.standing && !pullButtonPressed && !duckBehavior.ducking) ||
			(!faceDirVehavior.enabled && left && !collisionState.onPushBlock && collisionState.standing && !pullButtonPressed && !duckBehavior.ducking /*&& !collisionState.onLadder*/ && onStandBesideBlockDetected) ||
			(!faceDirVehavior.enabled && right && !collisionState.onPushBlock && collisionState.standing && !pullButtonPressed && !duckBehavior.ducking /* && !collisionState.onLadder*/&& onStandBesideBlockDetected)) {//escape
			//Debug.Log ("escape");
			//if (!collisionState.onRope) {
				//jumpBehavior.enabled = true;
				//duckBehavior.enabled = true;
				//ToggleScripts (true, this.GetType().ToString());
				//onPushBlockDetected = false;
				//onPullBlockDetected = false;
				//onGrabBlockDetected = false;
				OffPushing();
			//}

		} else if (	(left && inputState.direction == Directions.Right && collisionState.onPushBlock && collisionState.standing && pullButtonPressed && !duckBehavior.ducking) || //pull to left
					(right && inputState.direction == Directions.Left && collisionState.onPushBlock && collisionState.standing && pullButtonPressed && !duckBehavior.ducking)){ // pull to right
					
			//Debug.Log ("pull");
			onGrabBlockDetected = false;
			onPushBlockDetected = false;

//			if (equit.currentItem == 1) {
//				equit.currentItem = 0;
//			}

			if (!onPullBlockDetected) {
				jumpBehavior.enabled = false;
				duckBehavior.enabled = false;
				//ladderClimbingBehavior.enabled = false;
				ToggleScripts (false, this.GetType().ToString());
				onPullBlockDetected = true;
			}

			if (onPullBlockDetected) {
				if (!dist2d.enabled) {
					dist2d.enabled = true;
					dist2d.connectedBody = collisionState.onPushBlock.gameObject.GetComponent<Rigidbody2D> ();
					dist2d.autoConfigureDistance = true;
					//dist2d.distance = .4f;
					dist2d.enableCollision = false;
				}

				if (left || right) {
					float velX = 0f;
					velX = speed * (right ? 1 : -1);
					if (collisionState.onEnvironmentElement && collisionState.onEnvironmentElement.tag.ToLower ().Contains ("liquid")) {						
							velX /= 2f;
					}
					playerRigidBody2d.linearVelocity = new Vector2 (velX, playerRigidBody2d.linearVelocity.y);
				}
			}
		//just pressing "push/pull- button"
		} else if (!(right || left) && collisionState.onPushBlock && collisionState.standing && pullButtonPressed && !duckBehavior.ducking) {
			onGrabBlockDetected = true;
			onPushBlockDetected = false;
			onPullBlockDetected = false;
			onStandBesideBlockDetected = false;
			ToggleScripts (false, this.GetType().ToString());

		//just standing beside block
		} else if (!(right || left) && collisionState.onPushBlock && collisionState.standing && !pullButtonPressed && !duckBehavior.ducking) {			
			onGrabBlockDetected = false;
			onPushBlockDetected = false;
			onPullBlockDetected = false;
			onStandBesideBlockDetected = true;
		}
	}

	void OffPushing(){
		if (onPushBlockDetected || onPullBlockDetected || onGrabBlockDetected || onStandBesideBlockDetected) {
			dist2d.enabled = false;
			ToggleScripts (true, this.GetType ().ToString ());
			jumpBehavior.enabled = true;
			duckBehavior.enabled = true;
		
			onPushBlockDetected = false;
			onPullBlockDetected = false;
			onGrabBlockDetected = false;
			onStandBesideBlockDetected = false;
			//Debug.Log ("here");
		}
	}

	void OnDisable(){
		OffPushing();
	} 
}
