using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAtoB : MonoBehaviour {

	public bool isMoving;
	public float speed = 1f;
	public Vector3 B_point;
	public bool viseversa;
	public bool ignoreYAxis;
	public ObjectFaceDirection objectFaceDirection;

	private Vector3 A_point;
	private Color debugCollisionColor = Color.yellow;
	// Use this for initialization
	void Start () {
		A_point = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (ignoreYAxis)
			B_point.y = transform.position.y;
		
		float dir = 0f;
		dir = (B_point - transform.position).normalized.x;

		if (dir < 0f && isMoving) {
			if (objectFaceDirection)
				objectFaceDirection.direction = ObjectFaceDirection.FaceDirections.Left;
			//isMoving = true;

		} else if (dir > 0f && isMoving) {
			if (objectFaceDirection)
				objectFaceDirection.direction = ObjectFaceDirection.FaceDirections.Right;
			//isMoving = true;

		} else if (dir == 0f) {
			isMoving = false;
			if (viseversa) {
				B_point = A_point;
				A_point = transform.position;
			}
		}

		if (isMoving)
			transform.position = Vector2.MoveTowards (transform.position, B_point, speed * Time.deltaTime);
	}

	void OnDrawGizmos(){
		
		if (ignoreYAxis)
			B_point.y = transform.position.y;

		Gizmos.color = debugCollisionColor;
		var pos = B_point;
		Gizmos.DrawSphere (pos, .05f);
		Gizmos.DrawLine (transform.position, pos);
	}
}
