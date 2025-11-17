using UnityEngine;
using System.Collections;

public class FloatEffect : MonoBehaviour {

	public float startY;
	public float duration = .5f;
	public float multiplier = 2;

	// Use this for initialization
	void Start () {
		//startY = transform.localPosition.y;
	}

	void OnEnable(){
		//Debug.Log ("here.");
		startY = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float newY = startY + (Mathf.Cos (Time.time / duration * multiplier)) / 4;
		transform.localPosition = new Vector3 (transform.localPosition.x, newY, transform.localPosition.z);
	}
}
