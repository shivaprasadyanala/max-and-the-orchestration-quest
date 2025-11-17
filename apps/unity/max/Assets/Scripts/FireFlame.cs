using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlame : MonoBehaviour {

	public bool isOn = true;
	public float animationSpeed = 1;

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

		animator.speed = animationSpeed;
	}
}
