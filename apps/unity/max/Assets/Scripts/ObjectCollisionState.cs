using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollisionState : MonoBehaviour {

	public LayerMask groundLayers;

	public LayerMask nearLayer;
	public LayerMask middleLayer;
	public LayerMask farLayer;

	public Collider2D onGround;
	public Collider2D collidedObject;
	public Collider2D middleCollidedObject;
	public Collider2D nearCollidedObject;

	public Vector2 circlePosition = Vector2.zero;
	public float circleRadius = .1f;

	public Vector2 rectanglePosition = Vector2.zero;
	public Vector3 rectangleSize = Vector3.zero;

	public Vector2 middleRectanglePosition = Vector2.zero;
	public Vector3 middleRectangleSize = Vector3.zero;

	public Vector2 nearRectanglePosition = Vector2.zero;
	public Vector3 nearRectangleSize = Vector3.zero;

	public Color debugCollisionColor = Color.red;
	public bool isInAir;

	private float default_circleRadius;
	private Vector3 default_nearRectangleSize;
	// Use this for initialization
	void Start () {
		default_circleRadius = circleRadius;
		default_nearRectangleSize = nearRectangleSize;
	}

	// Update is called once per frame
	void FixedUpdate () {
		var pos = circlePosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		//onGround = Physics2D.OverlapCircle (pos, circleRadius, (1 << LayerMask.NameToLayer ("Solid")) | (1 << LayerMask.NameToLayer ("Default")));
		onGround = Physics2D.OverlapCircle (pos, circleRadius, groundLayers);

		pos = rectanglePosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		collidedObject = Physics2D.OverlapBox (pos, rectangleSize, 1, farLayer);

		pos = middleRectanglePosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		middleCollidedObject = Physics2D.OverlapBox (pos, middleRectangleSize, 1, middleLayer);

		pos = nearRectanglePosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		nearCollidedObject = Physics2D.OverlapBox (pos, nearRectangleSize, 1, nearLayer);

		if (!onGround)
			isInAir = true;
		else
			isInAir = false;
	}

	public void DisableCircleHitArea(bool disable){
		circleRadius = disable ? 0f : default_circleRadius;
	}
	public void DisableNearRectangleHitArea(bool disable){
		nearRectangleSize = disable ? Vector3.zero : default_nearRectangleSize;
	}

	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;

		//draw the circle
		var pos = circlePosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, circleRadius);

		//draw the lines
		var topPos = rectanglePosition;
		topPos.x += transform.position.x;
		topPos.y += transform.position.y;
		Gizmos.DrawWireCube (topPos, rectangleSize);

		topPos = middleRectanglePosition;
		topPos.x += transform.position.x;
		topPos.y += transform.position.y;
		Gizmos.DrawWireCube (topPos, middleRectangleSize);

		topPos = nearRectanglePosition;
		topPos.x += transform.position.x;
		topPos.y += transform.position.y;
		Gizmos.DrawWireCube (topPos, nearRectangleSize);
	}
}
