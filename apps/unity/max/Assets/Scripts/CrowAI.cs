using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class CrowAI : MonoBehaviour {

	public LayerMask collisionLayer1;
	public Vector2 collisionPosition = Vector2.zero;
	public float collisionRadius = .5f;
	public bool flying;

	private Collider2D target;
	private bool playerSeen = false;
	private Animator animator;
	private MoveAtoB moveAtoB;
	private Color debugCollisionColor = Color.red;

	private AudioPlayer audioPlayer;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		moveAtoB = GetComponent<MoveAtoB> ();

		audioPlayer = GetComponent<AudioPlayer> ();
		//audioPlayer = gameObject.AddComponent<AudioPlayer>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		var pos = collisionPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		target = Physics2D.OverlapCircle (pos, collisionRadius, collisionLayer1);

		if (target || flying) {			
			if (!playerSeen) {
				Flyaway ();
			}
		}
		if (playerSeen) {
			flying = true;
			if (!moveAtoB.isMoving) {
				flying = false;
				Destroy (gameObject);
			}
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;
		var pos = collisionPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, collisionRadius);
	}

	void Flyaway(){		
		animator.SetInteger ("AnimState", 5);
		moveAtoB.enabled = true;
		playerSeen = true;

		if (audioPlayer) {
			audioPlayer.Audio_Play_Independently_At_Index (0);
		}
			
		//Debug.Log ("hereeeeeee1");
	}

	void OnDestroy(){
		Destroy (gameObject);
	}
}