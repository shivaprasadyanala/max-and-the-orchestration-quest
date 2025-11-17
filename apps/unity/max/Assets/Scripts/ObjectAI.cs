using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAI : ObjectAbstractBehavior {

	public enum FollowPlayerWay{
		None=0,
		Walk,
		Run,
		WalkReverse
	}
		
	public GameObject target;
	public bool lookAtTarget;
	public bool followTarget;
	public FollowPlayerWay followWay = FollowPlayerWay.None;

	private bool timeToFollow;
	private ObjectWalk objectWalkBehavoir;
	private Collider2D foundTarget;

	// Use this for initialization
	void Start () {
		objectCollisionState = GetComponent<ObjectCollisionState> ();
		objectWalkBehavoir = GetComponent<ObjectWalk> ();
		objectCollisionState.farLayer = 1 << target.layer;
	}

	// Update is called once per frame
	void Update () {
		if (objectCollisionState.collidedObject) {
			foundTarget = objectCollisionState.collidedObject;
		}

		if (foundTarget) {

			// ignore player is is dead
			if (foundTarget.tag == "Player") {
				var targetPhysics = foundTarget.GetComponent<PlayerPhysics> ();
				if (targetPhysics.isDead) {
					enabled = false;
					return;
				}
			}

			// follow target
			if (objectWalkBehavoir.enabled) {
				if (objectCollisionState.middleCollidedObject) {
					timeToFollow = false;

				} else if (!objectCollisionState.middleCollidedObject && objectCollisionState.collidedObject) {
					timeToFollow = followTarget;
				}

				if (followWay == FollowPlayerWay.None)
					objectWalkBehavoir.walking = objectWalkBehavoir.running = objectWalkBehavoir.walkingReverse = false;

				else if (followWay == FollowPlayerWay.Walk)
					objectWalkBehavoir.walking = timeToFollow;

				else if (followWay == FollowPlayerWay.Run)
					objectWalkBehavoir.running = timeToFollow;

				else if (followWay == FollowPlayerWay.WalkReverse)
					objectWalkBehavoir.walkingReverse = timeToFollow;
			}


			// look at target
			if (lookAtTarget) {
				var dir = transform.position - foundTarget.transform.position;
				//Debug.Log (dir.x);
				if (dir.x > 0f && dir.x <= .06f) {
					objectWalkBehavoir.walking = false;

				} else if (dir.x < 0) {
					objectFaceDirectionBehavior.direction = ObjectFaceDirection.FaceDirections.Right;

				} else if(dir.x > 0) {
					objectFaceDirectionBehavior.direction = ObjectFaceDirection.FaceDirections.Left;
				}
			}
		}
	}

	void OnEnable(){
		var dire = GetComponent<Director> ();
		if (dire) {
			//dire.gameObject.SetActive (true);
			dire.enabled = true;
		}
	}
	void OnDisable(){
		var dire = GetComponent<Director>();
		if (dire) {
			//dire.gameObject.SetActive (false);
			dire.enabled = false;
		}
	}

//	void FixedUpdate(){
//		var pos = Vector2.zero;
//		pos.x += transform.position.x;
//		pos.y += transform.position.y;
//		var collidedTarget = Physics2D.OverlapCircle (pos, actionCircleRadius, objectCollisionState.targetLayer);
//		if (collidedTarget) {
//			if (attackTarget) {
//				objectActionBehavoir.acting = true;
//			}
//		}
//	}

//	void OnDrawGizmos(){
//		Gizmos.color = Color.magenta;
//
//		//draw the circle
//		var pos = Vector2.zero;
//		pos.x += transform.position.x;
//		pos.y += transform.position.y;
//		Gizmos.DrawWireSphere (pos, actionCircleRadius);
//	}
}
