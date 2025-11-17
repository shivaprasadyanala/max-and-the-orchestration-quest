using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Lifter : MonoBehaviour {

	public enum States
	{		
		Off=0,
		On,
		Unfunctional
	}
		
	public Vector3[] stopPoints;
	public int destinationPoint=0;
	public int currentPoint;
	public float speed=.5f;
	public bool isMoving = false;
	//public bool isOn = true;
	public States states;
	public Machine powerSupply;

	private Animator animator;
	private Color debugCollisionColor = Color.yellow;
	private float awaitingTime = 0f;
	private AudioPlayer audioPlayer;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		audioPlayer = GetComponent<AudioPlayer> ();

		currentPoint = destinationPoint;
		//move the lifter to first point
		if (stopPoints.Length > 0) {
			transform.position = stopPoints [0];
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (powerSupply) {
			if (powerSupply.machineState == Machine.MachineState.On && states == States.Unfunctional) {
				states = States.On;

			}else if (powerSupply.machineState == Machine.MachineState.Off) {
				states = States.Unfunctional;
			}
		} 

		if (states == States.Unfunctional) {
			//turn lights red
			ChangeAnimationState(0);
			isMoving = false;
			return;
		}

		// preventing runtime errors
		if (destinationPoint > stopPoints.Length - 1) {
			destinationPoint = stopPoints.Length - 1;
		} else if (destinationPoint < 0) {
			destinationPoint = 0;
		}

		// arrive to destination
		if (transform.position == stopPoints [destinationPoint]) {
			currentPoint = destinationPoint;
			// turn lights Green
			ChangeAnimationState (2);
			isMoving = false;
			awaitingTime = 0f;

			// Audio Stop Lifter
			if (audioPlayer.clipsIndexEnd != 2) {
				audioPlayer.clipsIndexBegin = 2;
				audioPlayer.clipsIndexEnd = 2;
				audioPlayer.isLoop = false;
				audioPlayer.Audio_Play_Clip ();
			}

		} else if (transform.position == stopPoints [currentPoint]) { 
			currentPoint = currentPoint < destinationPoint ? ++currentPoint : --currentPoint;

			// still moving
		} else {
			// turn lights Yellow
			ChangeAnimationState (1);

			// Audio Start Lifter
			if (!isMoving) {
				audioPlayer.clipsIndexBegin = 0;
				audioPlayer.clipsIndexEnd = 0;
				audioPlayer.isLoop = false;
				audioPlayer.Audio_Play_Clip ();
			}

			isMoving = true;
		}

		awaitingTime += Time.deltaTime;
		if (isMoving && awaitingTime >= 2f) {
			// moving towards
			transform.position = Vector2.MoveTowards (transform.position, stopPoints [currentPoint], speed * Time.deltaTime);

			// Audio Moving Lifter
			if (!audioPlayer.isPlaying) {
				audioPlayer.clipsIndexBegin = 1;
				audioPlayer.clipsIndexEnd = 1;
				audioPlayer.isLoop = true;
				audioPlayer.Audio_Play_Clip ();
			}
		}
	}

	void ChangeAnimationState(int state){
		if (animator) {
			if (animator.GetInteger ("AnimState") != state)
				animator.SetInteger ("AnimState", state);			
		}
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;
		target.transform.parent = transform;
	}
	void DoExitReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;

		target.transform.parent = null;
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


	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;

		if (stopPoints.Length == 0) {
			return;
		}

		Vector3 []points = stopPoints;
		//Array.Copy (stopPoints, points, stopPoints.Length);
		//points[destinationPoint] = transform.position;
		//int i = 0;
		//foreach (Vector3 stopPoint in stopPoints) {
		for (int i = 0; i < points.Length; i++) {
			Gizmos.DrawWireSphere (points[i], .2f);
			if (i > 0)
				Gizmos.DrawLine (points [i - 1], points [i]);
			//i++;
		}

		//draw the link to lever
//		if (theLever) {
//			Vector2 pos;
//			pos = theLever.transform.position;
//			Gizmos.color = debugCollisionColor;
//			Gizmos.DrawWireSphere (transform.position, .05f);
//			Gizmos.DrawLine (transform.position, pos);
//		}

	}
}
