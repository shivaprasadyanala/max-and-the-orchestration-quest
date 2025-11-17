using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePiece : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;	

		if (target.tag == "Player") {
			
			Walk walkBehaviour = target.GetComponent<Walk> ();



		
			if (walkBehaviour.running)
				DoBreakTheBridge ();
			
			if (targetBody2d.linearVelocity.y < -1f)
				DoBreakTheBridge ();
			
			Debug.Log(targetBody2d.linearVelocity.y);
		}
	}
	public void DoBreakTheBridge(){
		HingeJoint2D hingeJoint2d = GetComponent<HingeJoint2D> ();
		if (hingeJoint2d) {
			var ap = GetComponent<AudioPlayer> ();
			if (ap && hingeJoint2d.breakForce > 1f)
				ap.Audio_Play_Clip ();
			hingeJoint2d.breakForce = .1f;
		}
	}
	void DoExitReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		//GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;
	}
	void OnCollisionEnter2D(Collision2D target){
		DoReaction (target, null);
	}
	void OnTriggerEnter2D(Collider2D target){
		DoReaction (null, target);
	}
	void OnCollisionExit2D(Collision2D target){
		DoExitReaction (target, null);
	}
	void OnTriggerExit2D(Collider2D target){
		DoExitReaction (null, target);
	}
}
