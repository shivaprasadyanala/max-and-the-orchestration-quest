using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circularsaw : MonoBehaviour {

	public Switch3 theSwitch;
	public Switch theLever;

	public float maxRPM = 500.0f;
	public float currentRPM = 0f;
	public bool damaged;
	public bool isOn;

	public Smoke smoke; 
	private Smoke clone;

	private Animator animator;
	private Color debugCollisionColor = Color.green;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (theSwitch) {
			if (theSwitch.StateChanged) {
				if (theSwitch.state == Switch3.Switch3States.On) {
					isOn = true;
				} else if (theSwitch.state == Switch3.Switch3States.Off) {
					isOn = false;
				}
			}
		}
		if (theLever) {
			if (theLever.StateChanged) {
				//isOn = theLever.isOn;
			}
		}

		if (damaged)
			isOn = false;
		
		if (currentRPM >= 1) {
			
			GetComponent<CircleCollider2D> ().enabled = true;

		} else { //if it is off
			
			GetComponent<CircleCollider2D> ().enabled = false;
		}

		var ap = GetComponent<AudioPlayer> ();
		if (isOn) {
			if (currentRPM < maxRPM) {
				currentRPM += 2;
			}
			if (ap) {
				ap.clipsIndexBegin = 0;
				ap.clipsIndexBegin = 0;
				ap.isLoop = true;
				ap.Audio_Play_Clip ();
			}

		} else if (!isOn) {
			if (currentRPM > 0)
			{
				currentRPM -= 1;

				if (ap)
				{
					//ap.Audio_Stop_Clip ();
					ap.clipsIndexBegin = 2;
					ap.clipsIndexBegin = 2;
					ap.isLoop = false;
					ap.Audio_Play_Clip();
				}
			}
			else
			{
				ap.Audio_Stop_Clip();
			}		
		}

		//Rotation
		transform.Rotate (0f, 0f, 6.0f * currentRPM * Time.deltaTime);
	}

	float AngleDir(Vector2  a, Vector2 b){
		//return -a.x  + a.y ;
		var direction = System.Math.Round (((a - b)).x, 2);
		if (direction > 0.23f)
			return -1f;
		else if (direction < -0.23f)
			return 1f;
		else if (direction > -.23f && direction < .23f) {			
			return 1f;
		}
		return 0;
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){
		
		Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;


		if (isOn){
			var ap = GetComponent<AudioPlayer> ();
			if (ap) {
				//ap.clipsIndexBegin = 1;
				//ap.clipsIndexBegin = 1;
				ap.Audio_Play_Independently_At_Index (1);
			}

			float torque = Random.Range (25f, 45f) * Random.Range (-1.0f, 1.0f);
			float range1 = Random.Range (currentRPM/100f, currentRPM/99f) * AngleDir(transform.position, targetPosision);

			targetBody2d.AddForce (new Vector2 (range1, currentRPM/99f));
			targetBody2d.AddTorque (torque);
			//Debug.Log ("boom!!");
		}

		if (targetTag == "Player" && isOn) { //if hit the player be bloody
			animator.SetInteger ("AnimState", 1);

		} else if (targetTag == "Deadly-spyray") {//if spy ray hit stop and smoke
			Debug.Log("spy ray on saw");
			damaged = true;
			if (smoke && !clone) {				
				clone = Instantiate (smoke, transform.parent) as Smoke;
				clone.smokeScale = .6f;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D target){
		DoReaction (target, null);
	}

	void OnTriggerEnter2D(Collider2D target){
		DoReaction (null, target);
	}


	void OnDrawGizmos(){

		if (theSwitch) {
			Vector2 pos;
			pos = theSwitch.transform.position;
			Gizmos.color = debugCollisionColor;
			Gizmos.DrawWireSphere (transform.position, .05f);
			Gizmos.DrawLine (transform.position, pos);
		}

		if (theLever) {
			Vector2 pos;
			pos = theLever.transform.position;
			Gizmos.color = debugCollisionColor;
			Gizmos.DrawWireSphere (transform.position, .05f);
			Gizmos.DrawLine (transform.position, pos);
		}

	}
}
