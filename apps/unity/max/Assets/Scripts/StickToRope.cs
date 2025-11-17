using UnityEngine;
using System.Collections;

public class StickToRope : AbstractBehavior {

	public bool onRopeDetected;

	protected float defaultGravityScale;
	protected float defaultDrag;
	protected float defaultMass;


	protected FaceDirection faceDir;
	// Use this for initialization
	void Start () {
		defaultGravityScale = playerRigidBody2d.gravityScale;
		defaultDrag = playerRigidBody2d.linearDamping;
		defaultMass = playerRigidBody2d.mass;
		faceDir = GetComponent<FaceDirection> ();
	}

	// Update is called once per frame
	protected virtual void Update () {
		if (collisionState.onRope && !collisionState.standing) {
			if(!onRopeDetected){
				OnStick();
				ToggleScripts(false, this.GetType().ToString());
				onRopeDetected = true;
			}

		} else if (!collisionState.onRope || collisionState.standing) {
			if(onRopeDetected){
				OffRope();
				ToggleScripts(true, this.GetType().ToString());
				onRopeDetected = false;
			}
		}
	}

	protected virtual void OnStick(){
//		if (!collisionState.standing) {
//			body2d.gravityScale = 0;
//			body2d.drag = 50f;
//		}
	}

	protected virtual void OffRope(){
		//Debug.Log ("offrope base");
//		Debug.Log ("offrope");
//		playerRigidBody2d.gravityScale = defaultGravityScale;
//		playerRigidBody2d.drag = defaultDrag;
//		playerRigidBody2d.useAutoMass = true;
//		dist2d.enabled = false;
	}
}
