using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : AbstractBehavior {
	
	public bool onLedgeGrabbingDetected;
	public bool onLedgeClimbingUpDetected;
	public bool onLedgeStandingDetected;
	public bool onLedgeClimbingDownDetected;
	public bool onLedgeTeetering;

	protected float defaultGravityScale;
	protected float defaultDrag;

	private Vector2 LedgePosition;
	private bool reverseDirectionNeeded;
	private Ledge ledge;
	// Use this for initialization
	void Start () {
		defaultGravityScale = playerRigidBody2d.gravityScale;
		defaultDrag = playerRigidBody2d.linearDamping;
	}
		
	// Update is called once per frame
	protected virtual void Update () {
		
		var upButtonPressed = inputState.GetButtonValue (inputButtons [0]);
		var downButtonPressed = inputState.GetButtonValue (inputButtons [1]);

		//Debug.Log (!collisionState.onLedgeGrabbing && collisionState.onLedgeStanding && collisionState.standing);
		//grabbing ledge while jumping
		if (collisionState.onLedgeGrabbing && !collisionState.onLedgeStanding && !collisionState.standing) {
			if (!onLedgeGrabbingDetected && !onLedgeClimbingUpDetected) {
				ledge = collisionState.onLedgeGrabbing.GetComponent<Ledge> ();
				//Debug.Log ("grabbing...");
				upButtonPressed = false;
				downButtonPressed = false;

				OnGrab ();
			}

		//going down when standing on ledge 
		} else if (!collisionState.onLedgeGrabbing && collisionState.onLedgeStanding && collisionState.standing) {
			//Debug.Log (onLedgeClimbingDownDetected);
			if (!onLedgeClimbingDownDetected) {
				ledge = collisionState.onLedgeStanding.GetComponent<Ledge> ();

				if ((inputState.direction == Directions.Right && ledge.rightSideObject == null) ||
					(inputState.direction == Directions.Left && ledge.leftSideObject == null)) {
					reverseDirectionNeeded = true;
					//(Teetering) stopping player on the ledge to prevent falling down
					if (!onLedgeTeetering && !ledge.playerTeeteredOnce) {
						ToggleScripts (false, this.GetType().ToString());
						playerRigidBody2d.linearVelocity = Vector3.zero;
						//transform.position = new Vector3 ((ledge.transform.position.x ) + ledge.transform.localScale.x / 2, transform.position.y);
						playerRigidBody2d.Sleep ();
						inputState.isManual = true;
						onLedgeTeetering = true;

						//Debug.Log ("Teetering1");
					}

				} else if ((inputState.direction == Directions.Right && ledge.rightSideObject != null) ||
					(inputState.direction == Directions.Left && ledge.leftSideObject != null)) {
					reverseDirectionNeeded = false;
				}
				//downButtonPressed = false;

				if(ledge.playerTeeteredOnce)
					onLedgeStandingDetected = true;
			}

		} else {
			if (onLedgeStandingDetected || onLedgeGrabbingDetected || onLedgeTeetering) {
				onLedgeStandingDetected = false;
				onLedgeGrabbingDetected = false;
				onLedgeTeetering = false;
				//ledge.playerTeeteredOnce = true;
				//Debug.Log ("Teetering0000000");
			}
		}




		if (onLedgeGrabbingDetected) {

			if (upButtonPressed) {
				DoClimbLedge ();

			} else if (downButtonPressed) {
				OffLedge ();
			}

		} else if (onLedgeClimbingUpDetected) {
//			Debug.Log ("climbing...");

		} else if (onLedgeStandingDetected) {
			if (downButtonPressed) {
				ToggleScripts (false, this.GetType().ToString());
				inputState.isManual = true;
				if (reverseDirectionNeeded) {
					inputState.DoReverseDirection ();
					reverseDirectionNeeded = false;
					//Debug.Log ("INJA1...........................................................");
				}

				onLedgeClimbingDownDetected = true;
				onLedgeClimbingUpDetected = false;
				onLedgeStandingDetected = false;
				onLedgeTeetering = false;
				ledge.playerTeeteredOnce = true;
//				onLedgeGrabbingDetected = false;

				playerRigidBody2d.linearVelocity = Vector3.zero;
				playerRigidBody2d.gravityScale = 0;
				playerRigidBody2d.linearDamping = 0;
				if (collisionState.onLedgeStanding) {
					//Debug.Log (collisionState.onLedgeStanding.transform.position);
					LedgePosition = collisionState.onLedgeStanding.transform.position;
				}
				playerRigidBody2d.transform.position = new Vector2 (LedgePosition.x - .2f*(inputState.direction == Directions.Right ? 1 : -1), transform.position.y);
			}

		} else if (onLedgeClimbingDownDetected) {

			onLedgeClimbingUpDetected = false;
			onLedgeStandingDetected = false;
			onLedgeGrabbingDetected = false;
			onLedgeTeetering = false;
			ledge.playerTeeteredOnce = true;
			//Debug.Log ("climbing down...");
			var distPos = new Vector3(LedgePosition.x - .1f*(inputState.direction == Directions.Right ? 1 : -1), LedgePosition.y-.1f);
			transform.position += (distPos - transform.position) * 1.5f * Time.deltaTime;
		}

		// Priventer #1 :
		//if(!onLedgeClimbingDownDetected && !onLedgeClimbingUpDetected && !onLedgeGrabbingDetected && !onLedgeStandingDetected && !onLedgeTeetering && 
	}

	public void OnGrab(){
		//Debug.Log ("onGrab");
		onLedgeGrabbingDetected = true;
		onLedgeClimbingDownDetected = false;
		onLedgeStandingDetected = false;
		onLedgeClimbingUpDetected = false;
		onLedgeTeetering = false;
		//ledge.playerTeeteredOnce = true;

		ToggleScripts (false, this.GetType().ToString());
		inputState.isManual = true;

		playerRigidBody2d.gravityScale = 0;
		playerRigidBody2d.linearDamping = 0;
		if (collisionState.onLedgeGrabbing) {
			//Debug.Log (collisionState.onLedgeGrabbing.transform.position);
			LedgePosition = collisionState.onLedgeGrabbing.transform.position;
		}

		playerRigidBody2d.transform.position = new Vector2 (LedgePosition.x-.05f*(inputState.direction == Directions.Right ? 1 : -1), LedgePosition.y - .05f);
		playerRigidBody2d.linearVelocity = Vector2.zero;
	}

	public void OffLedge(){
		if (onLedgeGrabbingDetected || onLedgeClimbingUpDetected || onLedgeClimbingDownDetected || onLedgeTeetering) {
			ToggleScripts (true, this.GetType().ToString());
			inputState.isManual = false;

			playerRigidBody2d.gravityScale = defaultGravityScale;
			playerRigidBody2d.linearDamping = defaultDrag;
			playerRigidBody2d.transform.position = new Vector2 (LedgePosition.x, LedgePosition.y - .3f);
		}
		onLedgeGrabbingDetected = false;
		onLedgeStandingDetected = false;
		onLedgeClimbingDownDetected = false;
		onLedgeClimbingUpDetected = false;
		onLedgeTeetering = false;

		if(ledge)
			ledge.playerTeeteredOnce = true;


		//Debug.Log ("INJA2...");
	}

	public void ClimbLedge(){
		onLedgeClimbingUpDetected = false;
		onLedgeClimbingDownDetected = false;
		onLedgeGrabbingDetected = false;
		onLedgeStandingDetected = false;
		onLedgeTeetering = false;
		//ledge.playerTeeteredOnce = true;

		playerRigidBody2d.gravityScale = defaultGravityScale;
		playerRigidBody2d.linearDamping = defaultDrag;
		playerRigidBody2d.transform.position = new Vector2 (playerRigidBody2d.transform.position.x+.19f*(inputState.direction == Directions.Right ? 1 : -1), playerRigidBody2d.transform.position.y);
		//Debug.Log ("INJA1...");
		ToggleScripts (true, this.GetType().ToString());
		inputState.isManual = false;
	}

	public void DoClimbLedge(){
		onLedgeClimbingUpDetected = true;
		onLedgeGrabbingDetected = false;
		onLedgeClimbingDownDetected = false;
		onLedgeStandingDetected = false;
		onLedgeTeetering = false;
		//ledge.playerTeeteredOnce = true;

		inputState.isManual = true;
	}

	public void MoveLittleBitUp(){
		playerRigidBody2d.transform.position = new Vector2 (playerRigidBody2d.transform.position.x, playerRigidBody2d.transform.position.y + .05f);
	}

	public void MoveLittleBitDown(){
		//body2d.transform.position = new Vector2 (body2d.transform.position.x-.02f, body2d.transform.position.y - .08f);
	}

	public void EndofLedgeStopTeetering(){
		ToggleScripts (true, this.GetType().ToString());
		inputState.isManual = false;
		//onLedgeTeetering = false;
		ledge.playerTeeteredOnce = true;
		onLedgeStandingDetected = true;
	}

	void OnDisable(){
		OffLedge ();
	}
		
}
