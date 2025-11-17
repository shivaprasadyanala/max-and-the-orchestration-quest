using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slip : AbstractBehavior {

//	public float throwSpeed = 3f;
//	public bool onSlipDetected;
//
//	protected float defaultRotation;
//	protected float defaultGravityScale;
//	protected float defaultDrag;
//	private FaceDirection faceDir;
//	private CircleCollider2D cirC2d;
//	private BoxCollider2D boxC2d;
//
//	public GameObject dustPrefab;
//	public float dustSpawnDelay = .5f;
//	private float timeElapsed = 0f;
//	private float timeStart = 0f;

	// Use this for initialization
	void Start () {
//		//faceDir = GetComponent<FaceDirection> ();
//		cirC2d = GetComponent<CircleCollider2D> ();
//		boxC2d = GetComponent<BoxCollider2D> ();
//		defaultRotation = playerRigidBody2d.rotation;
//		defaultGravityScale = playerRigidBody2d.gravityScale;
//		defaultDrag = playerRigidBody2d.drag;	
	}

	// Update is called once per frame
	protected virtual void Update () {
//		if ((collisionState.onSlippyBackslash || collisionState.onSlippySlash)) {
//			if (!onSlipDetected) {				
//				OnSlip ();
//				ToggleScripts (false, this.GetType().ToString());
//				onSlipDetected = true;	
//			}
//		}else if (!collisionState.onSlippyBackslash && !collisionState.onSlippySlash) {
//			if (onSlipDetected) {
//				OffSlip ();
//				ToggleScripts (true, this.GetType().ToString());
//				onSlipDetected = false;
//			}
//		} else if (collisionState.standing) {
//			if (onSlipDetected) {
//				OffSlip ();
//				ToggleScripts (true, this.GetType().ToString());
//				onSlipDetected = false;
//			}
//		}
//
//		if((timeElapsed > dustSpawnDelay) && (timeStart >= .5f) && onSlipDetected){
//
//			if (dustPrefab) {
//				var dust = Instantiate (dustPrefab);
//				var pos = transform.position;
//				//pos.y += (collisionState.onSlippyBackslash ? 1 : 1);
//				pos.x += .1f * ((collisionState.onSlippyBackslash ? -1f : 1f));
//				dust.transform.position = pos;
//				dust.transform.localScale = transform.localScale;
//			}
//
//			timeElapsed = 0;
//
//		}
//
//		timeElapsed += Time.deltaTime;
//		timeStart += Time.deltaTime;
	}

	protected virtual void OnSlip(){
//		if((collisionState.onSlippyBackslash || collisionState.onSlippySlash)) {
//			boxC2d.enabled = true;
//			cirC2d.enabled = false;
//			//body2d.rotation = collisionState.onSlippyBackslash ? -.4f : .4f;
//			playerRigidBody2d.constraints = RigidbodyConstraints2D.None;
//			//faceDir.enabled = false;
//			playerRigidBody2d.gravityScale = 2f;
//			playerRigidBody2d.drag = 0f;
//
//			transform.localScale = new Vector3 (collisionState.onSlippyBackslash ? 1: -1, 1, 1);
//
//			//Debug.Log (collisionState.onSlippyBackslash + " " + collisionState.onSlippySlash);
//		}
	}

	protected virtual void OffSlip(){
//		if (playerRigidBody2d.gravityScale != defaultGravityScale) {
//			timeElapsed = 0;
//			//faceDir.enabled = true;
//			boxC2d.enabled = false;
//			cirC2d.enabled = true;
//			playerRigidBody2d.rotation = defaultRotation;
//			playerRigidBody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
//			playerRigidBody2d.gravityScale = defaultGravityScale;
//			//throw player after slip
//			//inputState.direction = collisionState.onSlippyBackslash ? Directions.Left : Directions.Right;
//			//body2d.velocity = new Vector2(throwSpeed * (collisionState.onSlippyBackslash ? -1 : 1), body2d.velocity.y);
//			playerRigidBody2d.drag = defaultDrag;
//
//
//			playerRigidBody2d.rotation = 0f;
//
//			//Debug.Log ("offslip done");
//		}
	}
}
