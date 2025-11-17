using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad_Number : MonoBehaviour {

	public int engNumber = 0;
	public float collisionRadius = .1f;

	private Collider2D target;
	private Color debugCollisionColor = Color.red;
	private Animator animator;
	private bool isStateChanged = false;
	private float timeElapsed = 0f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame

	void Update(){		

		if (animator.GetInteger ("AnimState") == 1) {
			timeElapsed += Time.deltaTime;
			if (timeElapsed >= .3f) {
				animator.SetInteger ("AnimState", 0);
				timeElapsed = 0f;
			}
		}

		var pos = Vector2.zero;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		target = Physics2D.OverlapCircle (pos, collisionRadius, 1 << LayerMask.NameToLayer ("Keypad_Knob"));

		if (target) {
			Keypad_Knob keypad_knob = target.GetComponent<Keypad_Knob> ();
			if (keypad_knob && keypad_knob.isOn && !isStateChanged) {
				
				animator.SetInteger ("AnimState", 1);
				keypad_knob.AppendSprite (GetComponent<Image> ().sprite);
				keypad_knob.finalNumber = keypad_knob.finalNumber * 10 + engNumber;

				isStateChanged = true;
				//Debug.Log ("number #: " + name + " hit by: " + target.name);

			} else if (!keypad_knob.isOn && isStateChanged) {
				isStateChanged = false;
			}
		}

	}

	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;
		var pos = Vector2.zero;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, collisionRadius);
	}
}
