using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheCamera : MonoBehaviour {

	public Vector2 whereOfCamera;

	public bool ignoreY = true;

	private Transform _t;
	//private Camera2DFollow camera2Dfollow;

	// Use this for initialization
	void Start () {		
		_t = Camera.main.transform;
		//camera2Dfollow = _t.GetComponent<Camera2DFollow> ();
	}
	
	// Update is called once per frame
	void Update () {
		var newY = ignoreY ? transform.position.y : _t.position.y + whereOfCamera.y;
		transform.position = new Vector3 (_t.position.x + whereOfCamera.x, newY, transform.position.z);

		//Debug.Log (camera2Dfollow.isDoingLookDown);
		//IgnoreY = camera2Dfollow.isDoingLookDown;
	}
}
