using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserfire : MonoBehaviour {

	private Laser laser; 

	// Use this for initialization
	void Start () {
		laser = this.transform.parent.GetComponent<Laser> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (laser) {
			var laserPointCount = laser.lineRenderer.positionCount;

			if (laserPointCount > 1) {
				if (laser.lineRenderer.GetPosition (laserPointCount - 1) != this.transform.position) {
					Destroy (gameObject);
				}
			} else if (laserPointCount == 0) {
				Destroy (gameObject);
			}
			
		}
	}
}
