using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTop : MonoBehaviour {

	private LadderClimbing ladderClimbingBehaviour;

	void Start(){
		ladderClimbingBehaviour = FindObjectOfType<LadderClimbing> ();
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;
		//Debug.Log(transform.parent.gameObject.name);
		var body2d = target.gameObject.GetComponent<Rigidbody2D>();
		if (body2d) {
			//Debug.Log (body2d.velocity.y);
			if (body2d.velocity.y >= 0) {
				var trans = transform.parent.Find ("LadderPlatform");
				LadderPlatform ladderPlatform = trans.GetComponent<LadderPlatform> ();
				if (ladderPlatform) {
					ladderPlatform.ToggleEnable (true);
					ladderClimbingBehaviour.OffLadder ();
				}
			}
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

	void OnTriggerStay2D(Collider2D target){
		DoReaction (null, target);
	}
}
