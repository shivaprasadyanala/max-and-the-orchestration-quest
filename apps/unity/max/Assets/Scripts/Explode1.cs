using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode1 : MonoBehaviour {

	public bool explode = true;
	public float explosionPower = 50f;
	public GameObject ExplosionLight;

	private Animator animator;
	private Light myLight;
	private CircleCollider2D circleCollider2D;
	// Use this for initialization
	void Start () {
		myLight =(Light) ExplosionLight.GetComponent("Light");
		circleCollider2D = GetComponent<CircleCollider2D> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (explode) {
			animator.SetInteger ("AnimState", 1);

			var ap = GetComponent<AudioPlayer> ();
			if (ap) {
				ap.Audio_Play_Clip ();
			}
		}
	}
		
	float AngleDir(Vector2  a, Vector2 b){
		//return -a.x  + a.y ;
		var direction = System.Math.Round (((a - b)).x, 2);
		if (direction > 0)
			return -1f;
		else if (direction < 0)
			return 1f;
		else if (direction == 0) {			
			return Random.Range (-1.0f, 1.0f);
		}
		return 0;
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;

		float torque = Random.Range (explosionPower - 25f, explosionPower - 5f) * Random.Range (-1.0f, 1.0f);
		float xRange = Random.Range (explosionPower - 20f, explosionPower) * AngleDir(transform.position, targetPosision);
		float yRange = Random.Range (explosionPower - 20f, explosionPower);

//		if (targetTag == "Player" || 
//			targetTag == "Block") {

		if (explode) {
			targetBody2d.AddForce (new Vector2 (xRange, yRange));
			targetBody2d.AddTorque (torque);
		}
//		}
	}

	void OnCollisionEnter2D(Collision2D target){
		DoReaction (target, null);
	}

	void OnTriggerEnter2D(Collider2D target){
		DoReaction (null, target);
	}

	void OnTriggerStay2D(Collider2D target){
		DoReaction (null, target);
	}

	void OnDestroy(){
		Destroy (gameObject);
	}

	void LightsOff(){
		myLight.enabled = false;
		circleCollider2D.enabled = false;
	}

	void LightsOn(){
		myLight.enabled = true;
		circleCollider2D.enabled = true;
	}
}
