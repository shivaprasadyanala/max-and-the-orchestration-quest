using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailObjectAI : MonoBehaviour {


	protected Vector3 velocity;
	public Transform _transform;
	public float distance = 1f;
	public float speed = 1f;
	private Vector3 _originalPosition;
	public bool isGoingLeft = false;
	private ObjectFaceDirection objectFaceDirection;

	public void Start () {
		_originalPosition = gameObject.transform.position;
		_transform = GetComponent<Transform>();
		velocity = new Vector3(speed,0,0);
		_transform.Translate ( velocity.x*Time.deltaTime,0,0);
		objectFaceDirection = GetComponent<ObjectFaceDirection> ();
	}

	void Update()
	{    
		float distFromStart = transform.position.x - _originalPosition.x;   

		if (isGoingLeft)
		{ 
			// If gone too far, switch direction
			if (distFromStart < -distance)
				SwitchDirection();

			_transform.Translate (velocity.x * Time.deltaTime, 0, 0);
			objectFaceDirection.direction = ObjectFaceDirection.FaceDirections.Left;
		}
		else
		{
			// If gone too far, switch direction
			if (distFromStart > distance)
				SwitchDirection();

			_transform.Translate (-velocity.x * Time.deltaTime, 0, 0);
			objectFaceDirection.direction = ObjectFaceDirection.FaceDirections.Right;
		}
	}

	void SwitchDirection()
	{
		isGoingLeft = !isGoingLeft;
		//TODO: Change facing direction, animation, etc
	}
}
