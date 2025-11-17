using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPlatform : MonoBehaviour {

	public enum FloatPlatformState
	{
		Closed=0,
		OpenMethod1,
		CloseMethod1
	}
	public FloatPlatformState state = FloatPlatformState.Closed;

	private Animator animator;
	private FloatPlatformState currentState;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		currentState = state;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (state != currentState) {
			animator.SetInteger ("AnimState", (int)state);
			currentState = state;
		}
	}
}
