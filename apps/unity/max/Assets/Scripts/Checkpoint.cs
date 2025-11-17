using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[System.Serializable]
public class Checkpoint {

	public bool isSaved;
	public int sceneIndex;
	public int checkpointIndex;

	public float playerPositionX;
	public float playerPositionY;
	public float playerPositionZ;

//	public float cameraPositionX;
//	public float cameraPositionY;
//	public float cameraPositionZ;

	public bool meterState;
	public float batteryAmount;

	public string zoneName;

	public Checkpoint(){
		isSaved = false;
		sceneIndex = 0;
		checkpointIndex = 0;
		playerPositionX = 0f;
		playerPositionY = 0f;
		playerPositionZ = 0f;
		meterState = false;
		batteryAmount = 0f;
		zoneName = string.Empty;
	}
}
