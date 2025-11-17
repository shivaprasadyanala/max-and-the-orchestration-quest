using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	public float zoomLevel;
	public float duration = 3f;

	private float timeElapsed = 0.0f;
	private float value1 = 1;

	void Start() {
		Camera.main.orthographicSize = 1.6f;
		value1 = Camera.main.orthographicSize;
		zoomLevel = Camera.main.orthographicSize;
	}

	void Update () {
		if (Camera.main.orthographicSize != zoomLevel) {
			if (zoomLevel <= 0f) {
				zoomLevel = .1f;
			}
			timeElapsed += Time.deltaTime / duration;
			//Debug.Log (value1 + " " + zoomLevel);
			Camera.main.orthographicSize = Mathf.Lerp (value1, zoomLevel, timeElapsed);

		} else {
			value1 = Camera.main.orthographicSize;
			timeElapsed = 0f;
		}
	}
}