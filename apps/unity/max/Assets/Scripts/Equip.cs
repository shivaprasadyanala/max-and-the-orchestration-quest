using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Equip : AbstractBehavior {

	public GameObject flashLight;
	private Light myLight;

	//private Animator animator;

	private Meter batteryMeter;

	float holdTime = 0f;

	[SerializeField]
	private int _currentItem = 0;
	public int currentItem {
		get{ return _currentItem; }
		set {
			_currentItem = value;
			if (_currentItem == 1)
				OnEquip ();
			else
				OffEquip ();
		}
	}
	private int default_currentItem = 0;

	void Start(){
		myLight = (Light) flashLight.GetComponent("Light");
		//myLight = GameObject.FindGameObjectWithTag ("PlayerFlashlight");
		batteryMeter = GameObject.FindObjectOfType<Meter> ();
	}

	override protected void Awake(){
		base.Awake ();
		//animator = GetComponent<Animator> ();
	}

	void OnEquip(){
		
		if (!batteryMeter || !batteryMeter.enabled) {
			return;
		}

		if (batteryMeter.battery > 0) {			
			myLight.enabled = true;
			animator.SetInteger ("EquippedItem", 1);

			Vibration.Vibrate (50);

			audioPlayer.priority = 1;
			audioPlayer.Audio_Play_Independently_At_Index(13);
		}
	}

	void OffEquip(){
		if(myLight)
			myLight.enabled = false;
		
		if(animator.gameObject.activeSelf)
			animator.SetInteger ("EquippedItem", 0);
	}

	void Update(){
		
		if (playerPhysicsBehavior.isDead) {
			myLight.enabled = false;
			if (batteryMeter) {
				batteryMeter.enabled = false;
			}
			return;
		}
			
		//var equipButton = inputState.GetButtonValue (inputButtons [0]);
		//var holdTime = inputState.GetButtonHoldTime (inputButtons [0]);
		var buttonPressed = CrossPlatformInputManager.GetButton("Interaction");

		if (buttonPressed) {
			holdTime += Time.deltaTime;
		} else {
			holdTime = 0f;
		}
		//Debug.Log (holdTime);

		if (buttonPressed && holdTime > 1.2f && inputState.inputEnabled) {
			holdTime = 0f;
			if (currentItem == 1)
				currentItem = 0;
			else
				currentItem = 1;
		}
	}

	void OnEnable(){
		currentItem = default_currentItem;
	}

	void OnDisable(){
		default_currentItem = currentItem;
		currentItem = 0;
	}
}