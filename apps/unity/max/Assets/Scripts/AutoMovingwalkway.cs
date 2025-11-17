using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovingwalkway : MonoBehaviour {

	public Vector2 speed;
	public bool isOn;

	private SurfaceEffector2D surfaceEffector2d;
	private AnimatedTexture animatedTexture;
	// Use this for initialization
	void Start () {
		surfaceEffector2d = GetComponent<SurfaceEffector2D> ();
		animatedTexture = GetComponent<AnimatedTexture> ();
		//speed = new Vector2 (.7f, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (!isOn) {
			if (surfaceEffector2d.speed < 0) {
				surfaceEffector2d.speed += .005f;
				surfaceEffector2d.speed = surfaceEffector2d.speed > 0 ? 0 : surfaceEffector2d.speed;
			}

			if (animatedTexture.speed.x > 0) {
				animatedTexture.speed = new Vector2 (animatedTexture.speed.x - .005f, animatedTexture.speed.y);
				animatedTexture.speed = animatedTexture.speed.x < 0 ? new Vector2(0, animatedTexture.speed.y) : animatedTexture.speed;
			}

		} else {
			animatedTexture.speed = speed;
			surfaceEffector2d.speed = -speed.x;
		}

//		if (surfaceEffector2d.speed == 0) {
//			surfaceEffector2d.enabled = false;
//		} else {
//			surfaceEffector2d.enabled = true;
//		}
	}
}
