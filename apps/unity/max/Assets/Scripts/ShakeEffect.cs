using UnityEngine;
using System.Collections;

public class ShakeEffect : MonoBehaviour {

	// vars
	public bool shakeOn = false;
	public float shakePower = .5f;
	public float shakingDuration = 3f;
	public bool isThisMovingObject;

	// sprite original position
	private Vector3 originPosition;
	private float timeElapsed = 0f;
	private float default_shakePower = 0f;
	// Use this for initialization
	void Start () {
		default_shakePower = shakePower;
	}

	// Update is called once per frame
	void Update () {

		// if shake is enabled
		if (shakeOn) {
			var ap = GetComponent<AudioPlayer> ();
			if (ap) {
				ap.Audio_Play_Clip ();
			}

			if (!isThisMovingObject) {
				// reset original position
				transform.position = originPosition;	
			}

			// generate random position in a 1 unit circle and add power
			Vector2 ShakePos = Random.insideUnitCircle * shakePower;

			// transform to new position adding the new coordinates
			transform.position = new Vector3 (transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z); 

			timeElapsed += Time.deltaTime;
			if (timeElapsed >= shakingDuration && shakingDuration != 0f) {
				shakePower -= .0005f;
				if (shakePower <= 0f) {
					shakeOn = false;

					shakePower = default_shakePower;
				}
			}

		} else {
			if (timeElapsed > 0f) {
				originPosition = transform.position;
				timeElapsed = 0f;
			}
		}
	}

	// shake on
	public void ShakeCameraOn(float sPower){

		//save position before start shake, 
		//this it's really important otherwise 
		//the sprite can goes away and will not return 
		//in native position
		originPosition = transform.position;

		//enable shaking and setting power
		shakeOn = true;
		shakePower = sPower;
	}

	// shake off
	public void ShakeCameraOff(){

		// shake off
//		shakeOn = false;

		// set original position after 
		transform.position = originPosition;
	}
		
}