using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShake : MonoBehaviour {

	public float ShakeY = 0.8f;
	public float ShakeYSpeed = 0.8f;


	public void setShake(float someY)
	{
		ShakeY = someY;
	}

	void Update()
	{
		Vector2 _newPosition = new Vector2(0, ShakeY);
		if (ShakeY < 0)
		{
			ShakeY *= ShakeYSpeed;
		}
		ShakeY = -ShakeY;
		transform.Translate(_newPosition, Space.Self);
	}
}