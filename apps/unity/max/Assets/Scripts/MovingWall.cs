using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour {

	private enum MovingWallState
	{
		Collapsed=0,
		Expanded
	}

	public Vector3 firstPoint;
	public Vector3 secondPoint;
	public float expandSpeed = .05f;
	public float collapseSpeed = .05f;

	public bool collapsed = true;
	private bool expanding;

	// Use this for initialization
	void Start () {
		firstPoint.x = transform.position.x;
		secondPoint.x = transform.position.x;

		if (collapsed) {
			transform.position = firstPoint;
		} else {
			transform.position = secondPoint;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!collapsed) {//expanding
			transform.position = Vector2.MoveTowards (transform.position, secondPoint, expandSpeed * Time.deltaTime);

		} else if (collapsed) {//collapsing
			transform.position = Vector2.MoveTowards (transform.position, firstPoint, collapseSpeed * Time.deltaTime);
		}

		if (transform.position == firstPoint && expandSpeed > 0) {
			collapsed = false;
//		} else if (transform.position == firstPoint) {
//			currentState = MovingWallState.Collapsed;
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;

		Gizmos.DrawLine (new Vector3 (firstPoint.x - transform.localScale.x, firstPoint.y, 0), new Vector3 (firstPoint.x + transform.localScale.x, firstPoint.y, 0));
		Gizmos.DrawLine (new Vector3 (secondPoint.x - transform.localScale.x, secondPoint.y, 0), new Vector3 (secondPoint.x + transform.localScale.x, secondPoint.y, 0));
	}
}
