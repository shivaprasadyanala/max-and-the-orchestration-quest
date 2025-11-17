using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

	public bool isOn;

	public LayerMask collisionLayer;
	public Vector3 collisionCubeSize;
	private Color debugCollisionColor = Color.red;

	public Vector3 collisionCubeCenterPosition = Vector3.zero;
	public Collider2D targetTrigger;
	//private bool triggerd = false;
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isOn) {
			animator.SetInteger ("AnimState", 1);
		} else {
			animator.SetInteger ("AnimState", 0);
		}
	}

	void FixedUpdate () {
		var pos = collisionCubeCenterPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		targetTrigger = Physics2D.OverlapBox(pos, collisionCubeSize, 1, collisionLayer);

		if (targetTrigger) {
			//triggerd = true;
			if(targetTrigger.tag.ToLower() == "wind")
				isOn = targetTrigger.GetComponent<Wind> ().isOn;
			//Debug.Log (targetTrigger.GetComponent<Wind> ().isOn);

		} else if (!targetTrigger) {
			isOn = false;
			//triggerd = false;
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;

		//draw hit area
		var bpos = collisionCubeCenterPosition;
		bpos.x += transform.position.x;
		bpos.y += transform.position.y;
		Gizmos.DrawWireCube (bpos, collisionCubeSize);
	}
}
