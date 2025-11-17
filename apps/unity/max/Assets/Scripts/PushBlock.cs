using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour {

//	public LayerMask EnvLayer;
	public GameObject forcedBySomething;
//	public Vector3 collisionCubeSize;
//	public Collider2D enviroment;

	private float timer = 0f;
//	private Color debugCollisionColor = Color.red;
//	private BoxCollider2D boxCollider2d;
//	private Rigidbody2D rigidBody2d;

	void Start(){
//		boxCollider2d = GetComponent<BoxCollider2D> ();
//		rigidBody2d = GetComponent<Rigidbody2D>();
	}

	void Update(){
		if (forcedBySomething) {
			timer += Time.deltaTime;
		}
		if (timer >= .5f) {
			forcedBySomething = null;
			timer = 0f;
		}
		//Debug.Log (rigidBody2d.velocity.x);
	}

	void FixedUpdate(){
//		Vector3 pos = Vector3.zero;
//		pos.x += transform.position.x;
//		pos.y += transform.position.y;
//		enviroment = Physics2D.OverlapBox (pos, collisionCubeSize, 0, EnvLayer);
//		if (enviroment) {
//			if (enviroment.tag.Contains ("Liquid")) {			
//				boxCollider2d.sharedMaterial = (PhysicsMaterial2D)Resources.Load("PhysicMaterials/ZeroFriction");
//
//			}
//		//Enviroment is air
//		} else {
//			boxCollider2d.sharedMaterial = (PhysicsMaterial2D)Resources.Load("PhysicMaterials/NormalFriction");
//		}
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;

		if (!targetTag.Contains ("Lifter") && !targetTag.StartsWith ("Switch") && targetTag != "Untagged" && targetTag != "Player" &&
			!targetTag.Contains("Ground") && targetTag != "Block" && !targetTag.ToLower ().Contains ("liquid")) {
			//Debug.Log ("enter");
			forcedBySomething = target;
			Debug.Log ("Pushblock forced by" + targetTag);

		} else if (targetTag.ToLower ().Contains ("liquid")) {
			//boxCollider2d.sharedMaterial = (PhysicsMaterial2D)Resources.Load ("PhysicMaterials/ZeroFriction");
		}
	}
	void DoExitReaction(Collision2D collisionTarget, Collider2D colliderTarget){
		
		//GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;

		//if (targetTag != "Untagged" && targetTag != "Player" && targetTag != "Ground") {
			//if(forcedBySomething)
			//forcedBySomething = null;
			//Debug.Log ("exit");
		//}

		if (targetTag.ToLower ().Contains ("liquid")) {
			//boxCollider2d.sharedMaterial = (PhysicsMaterial2D)Resources.Load ("PhysicMaterials/NormalFriction");
		}
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

//	void OnDrawGizmos(){
//		Gizmos.color = debugCollisionColor;
//
//		//draw hit area
//		Vector3 bpos = Vector3.zero;
//		bpos.x += transform.position.x;
//		bpos.y += transform.position.y;
//		Gizmos.DrawWireCube (bpos, collisionCubeSize);
//
//	}
}
