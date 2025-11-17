using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public abstract class AbstractBehavior : MonoBehaviour {

	public Buttons[] inputButtons;
	public MonoBehaviour[] dissableScripts;

	protected InputState inputState;
	protected Rigidbody2D playerRigidBody2d;
	protected CollisionState collisionState;
	protected DistanceJoint2D dist2d;
	protected AudioPlayer audioPlayer;
	protected PlayerPhysics playerPhysicsBehavior;
	protected Animator animator;

	private CircleCollider2D playerCircleCollider2d;
	private BoxCollider2D playerBoxCollider2d;

	private PhysicsMaterial2D _circleColliderSharedMaterial;
	public PhysicsMaterial2D circleColliderSharedMaterial{
		get{ return playerCircleCollider2d.sharedMaterial; }
		set{ playerCircleCollider2d.sharedMaterial = value; }
	}

	private bool _playerBoxColliderEnabled;
	public bool playerBoxColliderEnabled{
		get{ return playerBoxCollider2d.enabled; }
	}

	protected virtual void Awake(){
		animator = GetComponent<Animator> ();

		inputState = GetComponent<InputState> ();
		playerRigidBody2d = GetComponent<Rigidbody2D> ();
		playerCircleCollider2d = GetComponent<CircleCollider2D> ();
		playerBoxCollider2d = GetComponent<BoxCollider2D> ();
		collisionState = GetComponent<CollisionState> ();
		dist2d = GetComponent<DistanceJoint2D> ();
		audioPlayer = GetComponent<AudioPlayer> ();
		playerPhysicsBehavior = GetComponent<PlayerPhysics> ();
	}

	protected virtual void ToggleScripts(bool value, string callerName){

		if (playerPhysicsBehavior.isDead && value == true) {
			//Debug.Log ("player is DEAD and ''" + callerName + "'' trying to Enable its scripts...");
			return;
		}

		//Debug.Log ("''" + callerName + "'' is calling ToggleScripts (" + value + ")");
		foreach (var script in dissableScripts) {
			script.enabled = value;
		}
	}

	protected virtual void ToggleColliders(Collider2D collider, bool value, string callerName){

		if (playerPhysicsBehavior.isDead && playerPhysicsBehavior.dieWay == PlayerPhysics.DieWay.byPressing && value == true) {
			//Debug.Log ("player is DEAD and ''" + callerName + "'' trying to Enable its Collider...");
			return;
		}

		Debug.Log ("''" + callerName + "'' is setting " + collider.GetType().Name + " (" + value + ")");
		collider.enabled = value;
	}

	protected virtual void ToggleCollidersTrigger(Collider2D collider, bool value, string callerName){

		//if (playerPhysicsBehavior.isDead && playerPhysicsBehavior.dieWay == PlayerPhysics.DieWay.byPressing && value == true) {
			//Debug.Log ("player is DEAD and ''" + callerName + "'' trying to Set its Collider To isTrigger...");
			//return;
		//}

		//Debug.Log ("''" + callerName + "'' is setting isTrigger of " + collider.GetType().Name + " to (" + value + ")");
		collider.isTrigger = value;
	}
}
