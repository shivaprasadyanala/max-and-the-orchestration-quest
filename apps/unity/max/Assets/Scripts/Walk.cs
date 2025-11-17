using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Walk : AbstractBehavior {

	public float runningSpeed = 1f;
	public bool walking;
	public bool walkingReverse;
	public bool running;

	/// <summary>
	/// The is manual.
	/// </summary>
	private bool isManual = false;
	private float walkingThreshold = .7f;
	private LongJump jumpBehavior;
	private float tmpSpeed = 0f;

	// Use this for initialization
	void Start () {
		jumpBehavior = GetComponent<LongJump> ();
		running = false;
		walking = false;
	}

	// Update is called once per frame
	void Update () {
		
		var right = inputState.GetButtonValue (inputButtons [0]);
		var left = inputState.GetButtonValue (inputButtons [1]);
		//var walkButtonPressed = inputState.GetButtonValue (inputButtons [2]);

		tmpSpeed = runningSpeed;

		GameObject mjs = GameObject.Find ("MobileJoystick");
		if (mjs && !isManual) {
			Joystick j = mjs.GetComponent<Joystick> ();
			tmpSpeed = Mathf.Abs (j.delta_x / ((float)j.MovementRange));
		}
			
		if (!isManual && (right || left) && (collisionState.standing || jumpBehavior.isLongJumping || jumpBehavior.isShortJumping)) {

			if (tmpSpeed < walkingThreshold) {
				walking = true;
				running = false;
				tmpSpeed = runningSpeed / 2f;

			} else if (tmpSpeed > walkingThreshold) {
				running = true;
				walking = false;
			}

		} else if (!(right || left)) {
			OffWalk ();
		}

		if (walking || running || walkingReverse) {

			if ((right && collisionState.rightObject) || (left && collisionState.leftObject)) {
				running = walking = false;
				return;
			}

			if (isManual && (walking || walkingReverse))
				tmpSpeed = runningSpeed / 2f;

			OnWalk (runningSpeed);
		}
	}

	void OnWalk(float speed){
		
		float velX = 0f;

		//if on any type of Automoving-Walkway
		/////////////////////////////////////////////////////
		if (collisionState.standing)
		if (collisionState.standing.gameObject.tag.ToLower () == "automoving-walkway")
		if (collisionState.standing.GetComponent<AutoMovingwalkway> ().isOn) {
			//if on treadmill or something like that
			float automovingWalkwaySpeed = collisionState.standing.gameObject.GetComponent<SurfaceEffector2D> ().speed;
			tmpSpeed = tmpSpeed + (automovingWalkwaySpeed * (float)inputState.direction) + .2f;
		}
		/////////////////////////////////////////////////////

		//swimming under the water
		if (collisionState.onEnvironmentElement && collisionState.onEnvironmentElement.tag.ToLower ().Contains ("liquid") && collisionState.standing) {
			tmpSpeed /= 2f;

		} else if (collisionState.onEnvironmentElement && collisionState.onEnvironmentElement.tag.ToLower ().Contains ("liquid") && !collisionState.standing) {
			//Debug.Log ("here");
			tmpSpeed /= 3f;
		}

		velX = tmpSpeed * (float)inputState.direction;
		if (walkingReverse)
			velX = -velX;
		//Debug.Log ("velx :" + velX);
		playerRigidBody2d.linearVelocity = new Vector2 (velX, playerRigidBody2d.linearVelocity.y);

		// AUDIO walking
		audioPlayer.volume = 1f;
		audioPlayer.isLoop = false;
		if (collisionState.standing && collisionState.standing.tag.ToLower () == "ground-dirt") {
			audioPlayer.clipsIndexBegin = 0;
			audioPlayer.clipsIndexEnd = 3;

		} else if (collisionState.standing && collisionState.standing.tag.ToLower () == "ground-wood") {
			audioPlayer.clipsIndexBegin = 4;
			audioPlayer.clipsIndexEnd = 6;

		} else if (collisionState.standing && (collisionState.standing.tag.ToLower () == "ground-metal" || collisionState.standing.tag.ToLower ().Contains("automoving"))) {
			audioPlayer.clipsIndexBegin = 11;
			audioPlayer.clipsIndexEnd = 12;
		}
	}

	void OffWalk(){
		if (running || walking || walkingReverse) {
			ToggleScripts (true, this.GetType ().ToString ());

			if(!isManual)
				running = walking = false;
		}
	}

	public void DoWalking(string yes){
		isManual = bool.Parse (yes);
		walking = bool.Parse (yes);
		running = false;
	}
	public void DoWalkingReverse(string yes){
		isManual = bool.Parse (yes);
		walkingReverse = bool.Parse (yes);
		running = false;
	}
	public void DoRunning(string yes){
		isManual = bool.Parse (yes);
		running = bool.Parse (yes);
		walking = false;
	}

	void OnDisable(){
		OffWalk ();
	}
}
