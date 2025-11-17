using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLens : MonoBehaviour {
	
	public enum LensMode
	{
		Output=0,
		Input
	}

	public LensMode lensMode;
	public bool isOn = false;
	public bool isLaserArrived;

	private Laser laser;
	private CircleCollider2D cirCollider2d;
	private bool laserHasArrived = false;
	private float timeElapsed = 0f;
	// Use this for initialization
	void Start () {
		
		Transform trans = transform.Find ("LaserBeam");
		laser =  trans.GetComponent<Laser> ();

	}

	void Awake () {
		//Debug.Log (lensMode);
		cirCollider2d = GetComponent<CircleCollider2D> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (laser) {
			laser.isOn = isOn;
		}

		if (isOn) {
			lensMode = LensMode.Output;
		} else {
			lensMode = LensMode.Input;
		}

		if (lensMode == LensMode.Output && cirCollider2d.enabled) {
			cirCollider2d.enabled = false;
			//Debug.Log ("output");
		} else if (lensMode == LensMode.Input && !cirCollider2d.enabled) {
			cirCollider2d.enabled = true;
			//Debug.Log ("input");
		}

		if (laserHasArrived)
			timeElapsed += Time.deltaTime;
		if (timeElapsed >= 3f)
			isLaserArrived = true;
			
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;
		if (lensMode == LensMode.Input) {			
			Debug.Log (target.tag);
			if (target.tag.ToLower () == "deadly-laser") {
				laserHasArrived = true;
			}
		}
	}
	void DoExitReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;
		if (lensMode == LensMode.Input) {			
			Debug.Log (target.tag);
			if (target.tag.ToLower () == "deadly-laser") {
				laserHasArrived = false;
				isLaserArrived = false;
				timeElapsed = 0f;
			}
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
		
		
}
