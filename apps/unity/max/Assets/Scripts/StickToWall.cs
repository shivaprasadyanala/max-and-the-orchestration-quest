using UnityEngine;
using System.Collections;

public class StickToWall : AbstractBehavior {

	public bool onFenseDetected;
	public bool onLedgeDetected;

	protected float defaultGravityScale;
	protected float defaultDrag;

	// Use this for initialization
	void Start () {
		defaultGravityScale = playerRigidBody2d.gravityScale;
		defaultDrag = playerRigidBody2d.linearDamping;	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (collisionState.onFense && !collisionState.standing  && !collisionState.onLadder) {
			if (!onFenseDetected) {
				OnStick ();
				ToggleScripts (false, this.GetType().ToString());
				onFenseDetected = true;
			}
//		} else if (collisionState.onFense && !collisionState.standing  && !collisionState.onLadder) {
//			if (!onLedgeDetected) {
//				OffWall ();
//				ToggleScripts (true);
//				onLedgeDetected = true;
//				onFenseDetected = false;
//			}

		} else {
			if (onFenseDetected) {
				OffWall ();
				ToggleScripts (true, this.GetType().ToString());
				onFenseDetected = false;
			}
		}
	}

	protected virtual void OnStick(){		
//			body2d.gravityScale = 0;
//			body2d.drag = 50f;		
	}

	protected virtual void OffWall(){
//		if (body2d.gravityScale != defaultGravityScale) {
//			body2d.gravityScale = defaultGravityScale;
//			body2d.drag = defaultDrag;
//		}
	}
}
