using UnityEngine;
using System.Collections;

public class FaceDirection : AbstractBehavior {
	
	private CircleCollider2D circleCollider;

	// Use this for initialization
	void Start () {
		circleCollider = GetComponent<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		var right = inputState.GetButtonValue (inputButtons [0]);
		var left = inputState.GetButtonValue (inputButtons [1]);

		if (right && !inputState.isManual) {
			inputState.direction = Directions.Right;

		} else if (left && !inputState.isManual) {
			inputState.direction = Directions.Left;
		}

		//if player stuck between two pushblock
		if ((inputState.absVelX > -inputState.zeroNumber && inputState.absVelX < inputState.zeroNumber) && 
			collisionState.onPushBlock && !collisionState.standing && !(collisionState.onEnvironmentElement && collisionState.onEnvironmentElement.tag.ToLower().Contains("liquid"))) {
			circleCollider.enabled = false;

		} else if (collisionState.standing && !circleCollider.enabled) {
			circleCollider.enabled = true;
		}

		transform.localScale = new Vector3 ((float)inputState.direction, 1, 1);
	}
}
