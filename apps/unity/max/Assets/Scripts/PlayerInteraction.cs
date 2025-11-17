using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInteraction : AbstractBehavior {

	public bool interacted = false;

	float holdTime = 0f;

	void Update(){

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		var buttonPressed = inputState.GetButtonValue (inputButtons [0]);
		var holdTime = inputState.GetButtonHoldTime (inputButtons [0]);
		#elif UNITY_ANDROID || UNITY_IOS

		var buttonPressed = CrossPlatformInputManager.GetButtonDown("Interaction");
		if(!inputState.inputEnabled)
			buttonPressed = false;
		
		if(buttonPressed){
			holdTime += Time.deltaTime;
		}
		#endif

		// Debug.Log (buttonPressed +"---"+ holdTime);
		//if ((buttonPressed && holdTime < .0099f)) {
		if(buttonPressed){
			interacted = true;
		} else if(!buttonPressed){
			//Debug.Log ("false");
			interacted = false;
			holdTime = 0f;
		}
	}
}
