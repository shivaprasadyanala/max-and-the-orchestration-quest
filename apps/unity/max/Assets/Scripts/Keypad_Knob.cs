using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad_Knob : MonoBehaviour {

	public bool isOn;
	public GameObject []images;
	public int givenPassword;
	public Keypad keypad;
	public int finalNumber = 0;

	private int index = 0;
	private bool isCorrect = false;
	private AudioPlayer audioPlayer;
	private float timeElapsed = 0f;

	// Use this for initialization
	void Start () {
		audioPlayer = GetComponent<AudioPlayer> ();
	}

	// Update is called once per frame
	void Update () {
		if (isCorrect)
			timeElapsed += Time.deltaTime;
		if (timeElapsed >= 1f) {
			timeElapsed = 0f;
			CloseInterface ();
			this.transform.parent.parent.gameObject.SetActive (false);
		}

		if (finalNumber == givenPassword && !isCorrect) {
			isCorrect = true;
			keypad.isOn = true;
			audioPlayer.Audio_Play_Independently_At_Index (3);
		}
	}

	public void SetState(bool state){
		isOn = state;
	}
	public void AppendSprite(Sprite sprite){
		images [index].GetComponent<Image>().sprite = sprite;
		if (index < images.Length)
			index++;
	}
	public void DeleteSprite(){
		if (index == 0)
			return;
					
		images [--index].GetComponent<Image>().sprite = null;

		finalNumber = finalNumber / 10;
	}

	public void CloseInterface(){
		var player = GameObject.Find ("Player");
		if (player)
			player.GetComponent<InputState> ().inputEnabled = true;
		
		foreach (GameObject img in images)
			img.GetComponent<Image> ().sprite = null;
			
		finalNumber = 0;
		index = 0;
	}
}
