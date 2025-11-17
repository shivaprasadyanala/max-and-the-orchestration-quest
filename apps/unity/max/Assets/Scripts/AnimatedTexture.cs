using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour {

	public Vector2 speed = new Vector2 (-.1f, 0);

	private Vector2 offset = Vector2.zero;
	private Material material;

	// Use this for initialization
	void Start () {
		material = GetComponent<Renderer> ().material;

		//offset = material.GetTextureOffset ("_MainTex");
		offset = material.mainTextureOffset;
	}
	
	// Update is called once per frame
	void Update () {
		offset += speed * Time.deltaTime;

		//material.SetTextureOffset("_MainTex", offset);
		material.mainTextureOffset = offset;
	}
}
