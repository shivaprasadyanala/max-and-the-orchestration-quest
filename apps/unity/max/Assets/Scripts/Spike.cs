using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){
		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;
		var rigid2d = target.GetComponent<Rigidbody2D>();
		if (rigid2d) {
			if (targetTag.ToLower () == "player") {
				var ap = GetComponent<AudioPlayer> ();
				if (ap)
					ap.Audio_Play_Clip ();
				rigid2d.linearDamping = 20f;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D target){
		DoReaction (target, null);
	}
	void OnTriggerEnter2D(Collider2D target){
		DoReaction (null, target);
	}
	void OnCollisionStay2D(Collision2D target){
		//DoReaction (target, null);
	}
}
