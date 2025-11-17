using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

	public float dropFallingSpeed = 1f;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		animator.speed = dropFallingSpeed;
	}

	public void InstantiateDrop(){
		var drop = Instantiate (Resources.Load("drop-one"), transform.position, Quaternion.identity) as GameObject;
		drop.transform.parent = this.transform;
	}
}
