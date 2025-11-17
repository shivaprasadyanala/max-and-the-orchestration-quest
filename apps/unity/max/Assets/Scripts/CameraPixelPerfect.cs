using UnityEngine;
using System.Collections;

public class CameraPixelPerfect : MonoBehaviour {

    public Vector2 nativeResolution = new Vector2(480, 320);

    public float pixelsToUnits = 100f;
	public float scale = 100f;	

	void Awake () {
		var camera = GetComponent<Camera> ();

		if (camera.orthographic) {
			scale = Screen.height/nativeResolution.y;
			pixelsToUnits *= scale;
			camera.orthographicSize = (Screen.height / 2.0f) / pixelsToUnits;
		}
	}

}
