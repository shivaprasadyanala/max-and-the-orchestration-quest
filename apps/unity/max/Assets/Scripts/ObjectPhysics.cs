using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysics : ObjectAbstractBehavior {

	public AudioPlayer audioPlayer;

	public bool playDragSound = true;
	public int dragSoundIndexBegin = 0;
	public int dragSoundIndexEnd = 0;
	public bool playHitSound = true;
	public int hitSoundIndexBegin = 0;
	public int hitSoundIndexEnd = 0;
	public bool playFlySound = false;
	public int flySoundIndexBegin = 0;
	public int flySoundIndexEnd = 0;

	private bool isSliding;
	private bool isInAir = false;

	// Use this for initialization
	void Start () {
		if (!audioPlayer)
			audioPlayer = GetComponent<AudioPlayer> ();
	}
	
	// Update is called once per frame
	void Update () {
		//var ap = GetComponent<AudioPlayer> ();

		// Object Dragging
		if (objectCollisionState.onGround && (objectRigidbody2D.linearVelocity.x > .023f || objectRigidbody2D.linearVelocity.x < -.023f) && !isInAir && !isSliding) {
			isSliding = true;
			//Debug.Log ("slipping at vel = " + objectRigidbody2D.velocity.x);
			if (audioPlayer && playDragSound) {				
				audioPlayer.isLoop = true;
				audioPlayer.clipsIndexBegin = dragSoundIndexBegin;
				audioPlayer.clipsIndexEnd = dragSoundIndexEnd;
				audioPlayer.Audio_Play_Clip ();
			}
			
		} else if(isSliding && (objectRigidbody2D.linearVelocity.x < .023f && objectRigidbody2D.linearVelocity.x > -.023f)){
			isSliding = false;
			if (audioPlayer && playDragSound) {
				audioPlayer.isLoop = false;
				audioPlayer.Audio_Stop_Clip ();
			}
		}

		if (!objectCollisionState.onGround && (objectRigidbody2D.linearVelocity != Vector2.zero) && isInAir && !isSliding) {
			//Debug.Log ("flying at vel = " + objectRigidbody2D.velocity);
			if (audioPlayer && playFlySound) {
				audioPlayer.isLoop = true;
				audioPlayer.clipsIndexBegin = flySoundIndexBegin;
				audioPlayer.clipsIndexEnd = flySoundIndexEnd;
				audioPlayer.Audio_Play_Clip ();
			}

		}

		if (objectCollisionState.isInAir && !isInAir) {
			
			isInAir = true;
		}

		// Object Ground Hit
		if (objectCollisionState.onGround && isInAir) {
			isInAir = false;
			audioPlayer.clipsIndexBegin = hitSoundIndexBegin;
			audioPlayer.clipsIndexEnd = hitSoundIndexEnd;
			if (audioPlayer && playHitSound) {				
				audioPlayer.Audio_Play_Independently ();
			}
		}



	}
}
