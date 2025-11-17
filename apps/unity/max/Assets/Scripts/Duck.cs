using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Duck : AbstractBehavior {

	public float scale = .5f;
	public bool ducking;
	public float duckingDuration;
	public bool fourswalking;
	public float fourswalkSpeed = .2f;

	/// <summary>
	/// The is manual.
	/// </summary>
	private bool isManual = false;

	private Vector2 defaultCollisionStateLeftPosition;
	private Vector2 defaultCollisionStateRightPosition;
	private Vector3 defaultCollisionStateRectsSize;

	private FaceDirection faceDirBehaviuor;
	private bool scriptsDisabled;
	protected override void Awake(){
		base.Awake ();

		faceDirBehaviuor =  GetComponent<FaceDirection> ();
		defaultCollisionStateRectsSize = collisionState.rectsSize;
		defaultCollisionStateLeftPosition = collisionState.leftPosition;
		defaultCollisionStateRightPosition = collisionState.rightPosition;

		duckingDuration = 0f;
	}

	// Update is called once per frame
	void Update () {
		var right = inputState.GetButtonValue (inputButtons [0]);
		var left = inputState.GetButtonValue (inputButtons [1]);
		var down = inputState.GetButtonValue (inputButtons [2]);

		//ducking
		if (!(left || right) && down && collisionState.standing && !ducking && !isManual) {
			if (collisionState.onLadder_playerBottom)
				return;

			if (!ducking) {
				ducking = true;
				faceDirBehaviuor.enabled = true;
				OnDuck ();
				fourswalking = false;
			}

			//un duck
		} else if (collisionState.standing && ducking && !down && !collisionState.ceiling && !isManual) {
			ducking = false;
			fourswalking = false;
			OffDuck ();
			//Debug.Log ("un duck....");
			//fourswalk
		} else if ((right || left) && collisionState.standing && down && ducking && !(collisionState.onFense || collisionState.onPushBlock)) {			
			if (!fourswalking)
				fourswalking = true;
			//Debug.Log ("foursewalking....");
		//un fourswalk
		} else if ((right || left) && collisionState.standing && down && fourswalking && (collisionState.onFense || collisionState.onPushBlock)) {
			fourswalking = false;
			//Debug.Log ("un foursewalking");
			//if player stop Crowling but still remain ducking
		} else if ((!(right || left) && down) && !(collisionState.onFense || collisionState.onPushBlock) && ducking) {
			fourswalking = false;
		}

		if(ducking && isManual)
			OnDuck ();

		if (fourswalking && !(collisionState.onFense || collisionState.onPushBlock))
			OnFourswalk ();

		if((!fourswalking || (fourswalking && (collisionState.onFense || collisionState.onPushBlock)))&&scriptsDisabled)
			OffFourseWalk ();

		if (ducking && !fourswalking)
			duckingDuration += Time.deltaTime;
		else 
			duckingDuration = 0f;
	}

	protected virtual void OnDuck(){

		ToggleScripts (false, this.GetType ().ToString ());
		ToggleColliders (GetComponent<BoxCollider2D>(), false, this.GetType ().ToString ());

		collisionState.rightPosition = new Vector2 (collisionState.rightPosition.x, collisionState.rightPosition.y - .13f);
		collisionState.leftPosition = new Vector2 (collisionState.leftPosition.x, collisionState.leftPosition.y - .13f);
		collisionState.rectsSize = new Vector3 (collisionState.rectsSize.x, collisionState.rectsSize.y / 2f, collisionState.rectsSize.z);

	}

	protected virtual void OffDuck(){
		
		ToggleScripts (true, this.GetType ().ToString ());
		ToggleColliders (GetComponent<BoxCollider2D>(), true, this.GetType ().ToString ());

		collisionState.rectsSize = defaultCollisionStateRectsSize;
		collisionState.leftPosition = defaultCollisionStateLeftPosition;
		collisionState.rightPosition = defaultCollisionStateRightPosition;

//		if (inputState.isManual)
//			inputState.isManual = false;
	}

	protected virtual void OnFourswalk(){
		ToggleScripts (false, this.GetType ().ToString ());
		scriptsDisabled = true;

		var tmpSpeed = 0f;
		tmpSpeed = fourswalkSpeed;
		if(collisionState.standing) 
		if (collisionState.standing.gameObject.tag.ToLower () == "automoving-walkway"){
			//if on treadmill or somthing like that
			float automovingWalkwaySpeed = collisionState.standing.gameObject.GetComponent<SurfaceEffector2D> ().speed;
			tmpSpeed = tmpSpeed + (automovingWalkwaySpeed * (float)inputState.direction);
		}
		var velX = tmpSpeed * (float)inputState.direction;
		playerRigidBody2d.linearVelocity = new Vector2 (velX, playerRigidBody2d.linearVelocity.y);
	}

	protected virtual void OffFourseWalk(){
		fourswalking = false;
		if(scriptsDisabled)
			ToggleScripts (true, this.GetType ().ToString ());
		if (inputState.isManual)
			inputState.isManual = false;
		scriptsDisabled = false;
		//Debug.Log ("dis");
	}

	// methods to call from DIRCTOR
	public void DoDucking(string yes){
		isManual = bool.Parse (yes);
		ducking = bool.Parse (yes);
	}
	public void DoCrowling(string yes){
		isManual = bool.Parse (yes);
		fourswalking = bool.Parse (yes);
	}

	void OnDisable(){
		if (ducking || fourswalking) {
			OffDuck ();
			OffFourseWalk ();
		}
	}
}
