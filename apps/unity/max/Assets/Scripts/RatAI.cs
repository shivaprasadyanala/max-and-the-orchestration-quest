using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAI : MonoBehaviour {

	public LayerMask collisionLayer;
	public Vector2 collisionPosition = Vector2.zero;
	public float collisionRadius = .5f;

	private MoveAtoB moveAtoB;
	private Collider2D target;
	private Animator animator;
	private Color debugCollisionColor = Color.red;

	private AudioPlayer audioPlayer;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		moveAtoB = GetComponent<MoveAtoB> ();
		audioPlayer = GetComponent<AudioPlayer> ();
	}
		
	// Update is called once per frame
	void FixedUpdate(){
		var pos = collisionPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		target = Physics2D.OverlapCircle (pos, collisionRadius, collisionLayer);

		if (target) {
			if (!moveAtoB.isMoving) {
				animator.SetInteger ("AnimState", 1);
				moveAtoB.isMoving = true;

				//audioPlayer.Audio_Play_Independently_At_Position (gameObject);
				audioPlayer.Audio_Play_Independently_Randomly ("3");
			}
		} else {
			if (!moveAtoB.isMoving)
				animator.SetInteger ("AnimState", 0);
		}

		if (moveAtoB.isMoving) {
			
		} else {
			audioPlayer.Audio_Stop_Clip ();
		}
	}

//	void MoveToTarget()
//	{
//		if (Mathf.Round(Vector2.Distance (transform.position, B_point)) > 0f) {
//			Vector2 direction = (B_point - transform.position).normalized;
//			rigid2d.velocity = direction * speed;
//			Debug.Log ("direction="+Mathf.Round(Vector2.Distance (transform.position, B_point)));
//		}
//
//	}


	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;
		var pos = collisionPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, collisionRadius);
	}
}
