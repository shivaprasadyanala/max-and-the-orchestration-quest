using UnityEngine;
using System.Collections;

public class TiledBackground : MonoBehaviour {

//	public float textureSize = 32;
//	public bool scaleHorizontially = true;
//	public bool scaleVertically = true;

	public float newWidth ;
	//private float newHeight ;

	void Start(){
		if(newWidth == 0)
			newWidth = transform.localScale.x;
		//newHeight = transform.localScale.y;
	}

	// Use this for initialization
	void Update () {
	
//		var newWidth = !scaleHorizontially ? 1 : Mathf.Ceil (Screen.width / (textureSize * 1)/100);
//		var newHeight = !scaleVertically ? 1 : Mathf.Ceil (Screen.height / (textureSize * 1)/100);
//		transform.localScale = new Vector3 (newWidth*textureSize, newHeight*textureSize, 1);
//		Debug.Log (newWidth + "----"+ newHeight);
//		GetComponent<Renderer> ().material.mainTextureScale = new Vector3 (newWidth, newHeight, 1);

		//transform.localScale = new Vector3 (newWidth, newHeight, 1);
		GetComponent<Renderer> ().material.mainTextureScale = new Vector3 (newWidth, 1, 1);
	}

}
