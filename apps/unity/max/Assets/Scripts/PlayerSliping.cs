using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSliping : AbstractBehavior {

	public bool isOnSlippySlash;
	public bool isOnSlippyBackslash;

	private float timeElapsed = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//SLASH
		if (collisionState.standing && collisionState.standing.tag.ToLower () == "ground-slash" && !isOnSlippySlash
			&& (!collisionState.bottomBackObject || collisionState.bottomBackObject.tag != "Ground")) {
			OnSlippyGround (true);

		//BACK SLASH
		} else if (collisionState.standing && collisionState.standing.tag.ToLower () == "ground-backslash" && !isOnSlippyBackslash
			&& (!collisionState.bottomBackObject || collisionState.bottomBackObject.tag != "Ground")) {
			OnSlippyGround (false);

		} else if(!collisionState.standing || !collisionState.standing.tag.ToLower().Contains("slash") && (isOnSlippySlash || isOnSlippyBackslash)){
			timeElapsed += Time.deltaTime;
			if (timeElapsed >= 1f || (collisionState.standing && !collisionState.standing.tag.ToLower().Contains("slash"))) {
				OffSlipping ();

				timeElapsed = 0f;
			}
		}
			

		if (isOnSlippySlash) {
			inputState.direction = Directions.Left;

		} else if (isOnSlippyBackslash) {
			inputState.direction = Directions.Right;
		}
	}

	void OnSlippyGround(bool isSlash){
		ToggleScripts (false, this.GetType ().ToString ());
		inputState.isManual = true;

		isOnSlippySlash = isSlash;
		isOnSlippyBackslash = !isSlash;

		if (audioPlayer) {
			try{
				audioPlayer.clipsIndexBegin = 8;
				audioPlayer.clipsIndexEnd = 8;
				audioPlayer.volume = .5f;
				audioPlayer.isLoop = true;
				audioPlayer.Audio_Play_Clip ();
			}catch(Exception ex){
				Debug.Log ("Audio Error : " + ex.Message);
			}
		}
//		Debug.Log (inputState.direction + "  " + isOnSlippyBackslash);
//		if ((inputState.direction == Directions.Right && isOnSlippyBackslash) || 
//			(inputState.direction == Directions.Left && isOnSlippySlash)) {
//
//			if(collisionState.bottomBackObject)
//				playerRigidBody2d.AddForce (new Vector2 (5f * (float)inputState.direction, 5f));
//		}
	}

	void OffSlipping(){
		if (isOnSlippySlash || isOnSlippyBackslash) {
			isOnSlippySlash = isOnSlippyBackslash = false;
			ToggleScripts (true, this.GetType ().ToString ());
			inputState.isManual = false;

			audioPlayer.Audio_Stop_Clip ();
		}
	}

	void OnDisable(){
		OffSlipping ();
	}
}
