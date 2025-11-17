using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectExtras : ObjectAbstractBehavior {

	public bool stickToWall;

	public bool onStickDetected;

	private float timeElapsed = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (stickToWall) {
			if (objectCollisionState.nearCollidedObject && !onStickDetected) {
				onStickDetected = true;
				OnFense ();
				objectCollisionState.DisableCircleHitArea (true);

			} else if (objectCollisionState.onGround && onStickDetected) {
				onStickDetected = false;
				OffFense ();
				objectCollisionState.DisableNearRectangleHitArea (true);

			} else if (!objectCollisionState.nearCollidedObject && !objectCollisionState.onGround && onStickDetected) {
				onStickDetected = false;
				OffFense ();
				//objectCollisionState.DisableNearRectangleHitArea (true);
				objectRigidbody2D.AddForce (new Vector2 (0f, 100f));
			}

			if (onStickDetected) {
				objectRigidbody2D.linearVelocity = Vector2.zero;

				if (objectCollisionState.circleRadius == 0f) {
					timeElapsed += Time.deltaTime;
					if (timeElapsed >= .3f) {
						objectCollisionState.DisableCircleHitArea (false);
						timeElapsed = 0f;
					}
				} 

			} else {
				if (objectCollisionState.nearRectangleSize == Vector3.zero) {
					timeElapsed += Time.deltaTime;
					if (timeElapsed >= .3f) {
						objectCollisionState.DisableNearRectangleHitArea (false);
						timeElapsed = 0f;
					}
				}
			}
		}
	}


	void OnFense(){
		objectRigidbody2D.gravityScale = 0f;
		transform.rotation = Quaternion.Euler (0f, 0f, 90f * (float)objectFaceDirectionBehavior.direction);
	}

	void OffFense(){
		objectRigidbody2D.gravityScale = 1f;
		transform.rotation = Quaternion.Euler (0f, 0f, 0f);

		objectCollisionState.DisableCircleHitArea (false);
		objectCollisionState.DisableNearRectangleHitArea (false);
	}
}
