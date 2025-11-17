using UnityEngine;
using System.Collections;

public class WallSlide : StickToWall {

	public float slideVelocity = 1.5f;

	// Update is called once per frame
	override protected void Update () {
		base.Update ();

		var upButtonPressed = inputState.GetButtonValue (inputButtons [0]);
		var downButtonPressed = inputState.GetButtonValue (inputButtons [1]);

		if (onFenseDetected && !collisionState.standing && !collisionState.onLadder && !onLedgeDetected) {
			var velY = 0f;
			var velX = 0f;

			if (upButtonPressed) {
				velY = slideVelocity;

			} else if (downButtonPressed) {
				velY = slideVelocity * -1;
				//body2d.velocity = new Vector2(body2d.velocity.x, velY);
			}
				
			playerRigidBody2d.linearVelocity = new Vector2 (velX, velY);
		
		} else if (onFenseDetected && !collisionState.standing && onLedgeDetected) {
			Debug.Log ("ledge...");
		}

	}

	 override protected void OnStick(){		
		playerRigidBody2d.linearVelocity = Vector2.zero;
		playerRigidBody2d.gravityScale = 0;
		playerRigidBody2d.linearDamping = 50f;

		//Debug.Log ("onwallstick");
	}

	override protected void OffWall(){
		playerRigidBody2d.gravityScale = defaultGravityScale;
		playerRigidBody2d.linearDamping = defaultDrag;

	}
}
