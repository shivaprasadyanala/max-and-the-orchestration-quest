using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderPlatform : MonoBehaviour {

	private BoxCollider2D boxCollider;

	void Start(){
		boxCollider = GetComponent<BoxCollider2D> ();
	}

	public void ToggleEnable(bool enable){
		boxCollider.enabled = enable;
	}
}
