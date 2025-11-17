using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateManager : MonoBehaviour {

	private ObjectWalk objectWalkBehavoir;
	private ObjectJump objectJumpBehavoir;
	private ObjectFaceDirection objectFaceDirection;
	private Animator animator;

	private bool idle;
	private int prevAnimState, animState;
	private float animationspeed = 1f;
	private bool prevBehavoirState;
	// Use this for initialization
	void Start () {
		objectFaceDirection = GetComponent<ObjectFaceDirection> ();
		objectWalkBehavoir = GetComponent<ObjectWalk> ();
		objectJumpBehavoir = GetComponent<ObjectJump> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update(){

		//idle
		if ((objectJumpBehavoir && !objectJumpBehavoir.jumping || !objectJumpBehavoir) &&
			!objectWalkBehavoir.walking && !objectWalkBehavoir.running &&
			(objectFaceDirection && !objectFaceDirection.turning || !objectFaceDirection) &&
			!idle) {

			animationspeed = 1f;
			animState = 0;
			idle = true;
		}

		// Turn
		if (objectFaceDirection && objectFaceDirection.turning) {
			animState = objectFaceDirection.animStateTurn;
			idle = false;
			objectWalkBehavoir.walking = objectWalkBehavoir.running = false;
			animationspeed = 1f;

		//walk
		} else if (objectWalkBehavoir && objectWalkBehavoir.walking) {
			animState = objectWalkBehavoir.animStateWalking;
			idle = false;
			animationspeed = 1f;

			//run
		} else if (objectWalkBehavoir && objectWalkBehavoir.running) {
			animState = objectWalkBehavoir.animStateRunning;
			idle = false;
			animationspeed = (objectWalkBehavoir.animStateWalking == objectWalkBehavoir.animStateRunning ? (objectWalkBehavoir.runningSpeed * 1.5f) : 1f);

			// walking reverse
		} else if (objectWalkBehavoir && objectWalkBehavoir.walkingReverse) {
			animState = objectWalkBehavoir.animStateWalkingReverse;
			idle = false;
			animationspeed = 1f;
		}

		// kneeing before jump
		if (objectJumpBehavoir && objectJumpBehavoir.isJumpStartKneeing) {
			animState = objectJumpBehavoir.animStateJump_start;
			idle = false;
			animationspeed = 1f;

		//kneeing after jump
		} else if (objectJumpBehavoir && objectJumpBehavoir.isJumpEndKneeing) {
			animState = objectJumpBehavoir.animStateJump_end;
			idle = false;
			animationspeed = 1f;

		}
		//in the air
		if (objectJumpBehavoir && objectJumpBehavoir.isInAir) {
			animState = objectJumpBehavoir.animStateJump_inTheAir;
			idle = false;
			animationspeed = 1f;
		}

		if (animState != prevAnimState) {			
			animator.speed = animationspeed;
			animator.SetInteger ("AnimState", animState);
			prevAnimState = animState;
		}
	}
}
