using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

	public enum MachineState
	{
		Off=0,
		On,
		Unfunctional
	}
	public Switch theSwitch;
	public MachineState machineState = MachineState.Off;

	public AudioClip startupSound;
	public AudioClip runningSound;
	public AudioClip shutdownSound;

	private AudioSource audioSrc;
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		audioSrc = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(animator)
		if (animator.GetInteger ("AnimState") != (int)machineState) {
			animator.SetInteger ("AnimState", (int)machineState);
		}

		if(theSwitch)
			machineState = (MachineState)theSwitch.state;
	}

	void PlayStartup_Sound(){
		if (audioSrc.clip != startupSound) {
			audioSrc.clip = startupSound;
			audioSrc.loop = false;
			audioSrc.Play ();
		}
	}

	void PlayRunning_Sound(){				
		if (audioSrc.clip != runningSound) {
			audioSrc.clip = runningSound;
			audioSrc.loop = true;
			audioSrc.Play ();
		}
	}

	void PlayShutdownSound(){
		if (audioSrc.clip != shutdownSound) {
			audioSrc.clip = shutdownSound;
			audioSrc.loop = false;
			audioSrc.Play ();
		}
	}

	void StopSound(){
		if (audioSrc.clip == startupSound) {
			audioSrc.Stop ();
			audioSrc.clip = null;
		}
	}

}
