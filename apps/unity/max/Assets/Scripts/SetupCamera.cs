using UnityEngine;

using System.Collections;

public class SetupCamera : MonoBehaviour {

	public int targetWidth=400;
	public float pixelPerUnit=100;
	private Camera camera1;

	void Update(){
		camera1 = this.GetComponent<Camera> ();
		//int height = Mathf.RoundToInt (targetWidth / (float)Screen.width * Screen.height);
		camera1.orthographicSize = Screen.height / pixelPerUnit / 2;
		//Debug.Log (Screen.height);
	}
}
