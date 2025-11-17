using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class RopeSlide : StickToRope {

	public float slideUpVelocity = 1.5f;
	public float slideDownVelocity = -5f;
	public float swingVelocity = 1f;
	public bool slidingUpDetected;
	public bool slidingDownDetected;
	public bool swingingDetected;

	private Collider2D curRope;
	private bool swingedLeft;
	private bool swingedRight;
	private float timeElapsed = 0f;

	// Update is called once per frame
	override protected void Update () {
		base.Update ();

		var velY = 0f;
		var velX = 0f;

		var slideUp = inputState.GetButtonValue (inputButtons [0]);
		var slideDown = inputState.GetButtonValue (inputButtons [1]);
		var swapRightButton = inputState.GetButtonValue (inputButtons [2]);
		//var swingRightButtonHoldTime = inputState.GetButtonHoldTime (inputButtons [2])
		var swapLeftButton = inputState.GetButtonValue (inputButtons [3]);
		//var swingLeftButtonHoldTime = inputState.GetButtonHoldTime (inputButtons [3]);

		if (onRopeDetected && !collisionState.standing) {

			if (slideUp) {//slide up
				velY = slideUpVelocity;
				velX = Random.Range (0f, swingVelocity) * (Random.Range (0, 1) * -1);
				StickToNextChainLink ();

				slidingUpDetected = true;
				slidingDownDetected = false;

			} else if (slideDown) {//slide down
				velY = slideDownVelocity * -1;
				StickToNextChainLink ();

				slidingUpDetected = false;
				slidingDownDetected = true;
			
			} else if (swapRightButton && !swingedRight) {//swing to right
				//swingedRight = true;
				playerRigidBody2d.AddForce (new Vector2 (swingVelocity, 0));
				//Debug.Log("swing to left");
				timeElapsed += Time.deltaTime;

				if (inputState.direction == Directions.Left) {
					swingingDetected = true;
				}

			} else if (swapLeftButton && !swingedLeft) {//swing to left
				//swingedLeft = true;
				playerRigidBody2d.AddForce (new Vector2 (-swingVelocity, 0));
				//Debug.Log("swing to left");
				timeElapsed += Time.deltaTime;

				if (inputState.direction == Directions.Right) {
					swingingDetected = true;
				}
			}

			if (!slideUp && !slideDown) {
				slidingDownDetected = slidingUpDetected = false;
			}

			if (!swapLeftButton && !swapRightButton) {
				swingedLeft = swingedRight = false;
				timeElapsed = 0f;
			}

			if (timeElapsed >= .8f) { // 800 miliseconds
				swingedLeft = swingedRight = true;
				swingingDetected = false;
			}

			playerRigidBody2d.linearVelocity = new Vector2 (velX, velY);

		} else if(collisionState.standing){
			if (onRopeDetected)
				OffRope ();
		}
	}

	override protected void OnStick(){		
		StickToNextChainLink ();

		playerRigidBody2d.linearVelocity = Vector2.zero;
		playerRigidBody2d.gravityScale = 1f;
		playerRigidBody2d.linearDamping = 50f;
		playerRigidBody2d.useAutoMass = false;
		playerRigidBody2d.mass = 0.01f;

		//swappedToRight = false;
		//swappedToLeft = false;
	}

	override protected void OffRope(){
		if (onRopeDetected) {
			onRopeDetected = false;
			slidingDownDetected = slidingUpDetected = swingingDetected = false;
			playerRigidBody2d.gravityScale = defaultGravityScale;
			playerRigidBody2d.linearDamping = defaultDrag;
			playerRigidBody2d.useAutoMass = true;
			dist2d.enabled = false;
			//Debug.Log ("offrope");
		}
	}

	void StickToNextChainLink(){
		if (collisionState.onRope.gameObject.tag == "ChainLink" && collisionState.onRope != curRope) {

			dist2d.enabled = true;
			dist2d.connectedBody = collisionState.onRope.gameObject.GetComponent<Rigidbody2D> ();
			dist2d.autoConfigureDistance = false;
			dist2d.distance = .2f;
			dist2d.enableCollision = false;

			curRope = collisionState.onRope;

			//Debug.Log ("join");
		}
	}

	void OnDisable(){
		OffRope ();
	}
}
