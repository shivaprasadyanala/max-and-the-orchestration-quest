using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectAbstractBehavior : MonoBehaviour {

	public MonoBehaviour[] disableScripts;

	protected ObjectCollisionState objectCollisionState;
	protected Rigidbody2D objectRigidbody2D;
	protected ObjectExtras objectExtraBehavior;
	protected ObjectFaceDirection objectFaceDirectionBehavior;

	// Use this for initialization
	protected virtual void Awake(){
		objectCollisionState = GetComponent<ObjectCollisionState> ();
		objectRigidbody2D = GetComponent<Rigidbody2D> ();
		objectExtraBehavior = GetComponent<ObjectExtras> ();
		objectFaceDirectionBehavior = GetComponent<ObjectFaceDirection> ();
	}

	protected virtual void ToggleScripts(bool value, string callerName){
		//Debug.Log ("''" + callerName + "'' is calling ToggleScripts (" + value + ")");
		foreach (var script in disableScripts) {
			script.enabled = value;
		}
	}
}
