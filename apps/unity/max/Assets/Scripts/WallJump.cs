using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class WallJump : StickToWall {

	public Vector2 jumpVelocity = new Vector2(2.5f, 2.5f);
	public Vector2 letGoVelocity = new Vector2(1, 0);
	public bool jumpingOffWall;
	public float resetDelay = .2f;

	private float timeElapsed = 0;

	void start(){
		
	}

	// Update is called once per frame
	protected override void Update () {
	
		if (collisionState.onFense && !collisionState.standing && !collisionState.onLadder) {

			//var jumpButtonPressed = inputState.GetButtonValue(inputButtons[0]);
			var jumpButtonPressed = CrossPlatformInputManager.GetButtonDown("Jump");
			var leftButtonPressed = inputState.GetButtonValue(inputButtons[1]);
			var rightButtonPressed = inputState.GetButtonValue(inputButtons[2]);
			//Debug.Log (inputState.GetButtonValue(inputButtons[0]));
			if((jumpButtonPressed || 
				(inputState.direction == Directions.Right && leftButtonPressed) || 
				(inputState.direction == Directions.Left && rightButtonPressed)) && !jumpingOffWall){
				
				inputState.direction = inputState.direction == Directions.Right ? Directions.Left : Directions.Right;

				playerRigidBody2d.gravityScale = defaultGravityScale;
				playerRigidBody2d.linearDamping = defaultDrag;

				if (leftButtonPressed || rightButtonPressed && !jumpButtonPressed) {
					playerRigidBody2d.linearVelocity = new Vector2 (letGoVelocity.x * (float)inputState.direction, letGoVelocity.y);

				} else if (!leftButtonPressed && !rightButtonPressed && jumpButtonPressed) {
					playerRigidBody2d.linearVelocity = new Vector2 (jumpVelocity.x * (float)inputState.direction, jumpVelocity.y);

				} else if ((leftButtonPressed || rightButtonPressed) && jumpButtonPressed) {
					return;
				}
				ToggleScripts (false, this.GetType().ToString());
				jumpingOffWall = true;
			}

		}

		if (jumpingOffWall) {
			timeElapsed += Time.deltaTime;

			if(timeElapsed > resetDelay){
				ToggleScripts(true, this.GetType().ToString());
				jumpingOffWall = false;
				timeElapsed = 0;
			}
		}

	}
}
