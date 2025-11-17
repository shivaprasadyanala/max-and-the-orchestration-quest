using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {

	public bool isOn;
	public float flow;

	private float flowValue;
	private BoxCollider2D boxCollider2d;
	private BuoyancyEffector2D buoyancyEffector2d;

	private Vector3 collisionCubeSize;
	private Color debugCollisionColor = Color.green;
	// Use this for initialization
	void Start () {
		buoyancyEffector2d = GetComponent<BuoyancyEffector2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		buoyancyEffector2d.enabled = isOn;

		var ap = GetComponent<AudioPlayer> ();
		if (isOn) {			
			if (ap)
				ap.Audio_Play_Clip ();
			
		} else {
			ap.Audio_Stop_Clip_Smoothly ();
		}

		buoyancyEffector2d.flowAngle = flow;
		buoyancyEffector2d.flowMagnitude = flow;
	}

	void OnDrawGizmos(){
		boxCollider2d = GetComponent<BoxCollider2D> ();
		collisionCubeSize = boxCollider2d.bounds.size;

		Color color  = debugCollisionColor;
		color.a = .15f;
		Gizmos.color = color;
		//draw hit area
		var bpos = Vector3.zero;
		bpos.x += transform.position.x;
		bpos.y += transform.position.y;
		Gizmos.DrawCube (bpos, collisionCubeSize);
	}
}
