using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {

	public LayerMask Layer;
	public Vector3 collisionCubeSize;

	private Collider2D target;
	private Color debugCollisionColor = Color.red;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var pos = Vector3.zero;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		target = Physics2D.OverlapBox(pos, collisionCubeSize, 1, Layer);
		if (target) {
			Destroy (target.gameObject);
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;
		//draw hit area
		var bpos = Vector3.zero;
		bpos.x += transform.position.x;
		bpos.y += transform.position.y;
		Gizmos.DrawCube (bpos, collisionCubeSize);
	}
}
