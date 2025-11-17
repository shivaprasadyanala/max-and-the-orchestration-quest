using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWalk : ObjectAbstractBehavior {

	public float runningSpeed = 1f;
	public float walkingSpeed = .5f;
	public int animStateWalking = 0;
	public int animStateRunning = 0;
	public int animStateWalkingReverse = 0;

	public bool walking;
	public bool running;
	public bool walkingReverse;

	private bool isAnimationPlayed;
	private int prevAnimationStateNumber;
	// Use this for initialization
	void Start () {		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		float vel = runningSpeed * (objectFaceDirectionBehavior != null ? (float)objectFaceDirectionBehavior.direction : 1);

		if (walking || walkingReverse) 
			vel = walkingSpeed * (objectFaceDirectionBehavior != null ? (float)objectFaceDirectionBehavior.direction : 1);
		
		//Debug.Log ("velx :" + velX);

		if (walking || running || walkingReverse) {

			if (walkingReverse) 
				vel = -vel;

			Vector2 newVel = Vector2.zero;
			if(objectExtraBehavior && objectExtraBehavior.onStickDetected)
				newVel = new Vector2 (objectRigidbody2D.linearVelocity.x, vel * Mathf.Sin(transform.rotation.z));
			else
				newVel = new Vector2 (vel, objectRigidbody2D.linearVelocity.y);

			objectRigidbody2D.linearVelocity = newVel;
		} 
	}

	void OnDisable(){
		walking = running = false;
	}
}
