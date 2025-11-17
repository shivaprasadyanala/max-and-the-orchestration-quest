using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject target;

	private Transform _t;

	// Use this for initialization
	void Start () {
		if (!target) {
			target = GameObject.FindGameObjectWithTag ("Player");
		}
		_t = target.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (_t.position.x, _t.position.y+0.8f, transform.position.z);
	}
}
