using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour {

	public LayerMask groundLayer;

	public Vector2 rightPosition = new Vector2(.15f, 0);
	public Vector2 leftPosition = new Vector2(-.15f, 0);
	public Collider2D leftSideObject;
	public Collider2D rightSideObject;
	public bool playerTeeteredOnce;

	private Vector3 collisionCubeSize;
	private BoxCollider2D boxCollider2d;
	private Color debugCollisionColor = Color.red;

	// Use this for initialization
	void Start () {
		boxCollider2d = GetComponent<BoxCollider2D> ();
		collisionCubeSize = boxCollider2d.bounds.size;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var pos = leftPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		leftSideObject = Physics2D.OverlapCircle (pos, .04f, groundLayer);

		pos = rightPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		rightSideObject = Physics2D.OverlapCircle (pos, .04f, groundLayer);
	}
		
	void OnDrawGizmos(){

		//draw left and right points hit area
		Gizmos.color = debugCollisionColor;

		var pos = rightPosition;;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, .04f);
		pos = leftPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, .04f);

		//draw hit area
		boxCollider2d = GetComponent<BoxCollider2D> ();
		collisionCubeSize = boxCollider2d.bounds.size;

		Color color  = debugCollisionColor;
		color.a = .4f;
		Gizmos.color = color;

		var bpos = Vector3.zero;
		bpos.x += transform.position.x;
		bpos.y += transform.position.y;
		Gizmos.DrawCube (bpos, collisionCubeSize);
	}
}
