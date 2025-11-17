using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {

	public bool isOn = true;
	private ParticleSystem myParticleSystem;
	/// <summary>
	/// The duration (seconds).
	/// </summary>
	public float duration = 10f;
	public float smokeScale = 1f;
	private float timeElapsed = 0f;

	// Use this for initialization
	void Start () {
		myParticleSystem = GetComponentInChildren<ParticleSystem>();
	}

	void Awake(){		
	}

	// Update is called once per frame
	void Update () {
		myParticleSystem.transform.localScale = new Vector3 (smokeScale, smokeScale, smokeScale);

		if (timeElapsed >= duration && duration != 0) {
			myParticleSystem.Stop ();
			//Destroy (gameObject);
		}

		if (!isOn)
			myParticleSystem.Stop ();
		
		else {
			timeElapsed += Time.deltaTime;
			if ((duration != 0 && timeElapsed < duration) || duration == 0) 
				if(myParticleSystem.isStopped)
					myParticleSystem.Play ();
		}
	}
}
