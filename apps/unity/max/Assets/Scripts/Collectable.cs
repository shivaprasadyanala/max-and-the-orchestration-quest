using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public LayerMask collisionLayer;
	public Vector2 collisionPosition = Vector2.zero;
	public float collisionRadius = .5f;
	public float destroyAfter = 4f;

	private Collider2D target;
	private Color debugCollisionColor = Color.red;
	private DirectorWorker directorWorker;
	public float timeElapsed = 0f;
//	void OnTriggerEnter2D(Collider2D target){
//		if (target.gameObject.tag == targetTag) {
//			OnCollect(target.gameObject);
//			OnDestroy();
//		}
//	}

	void Start(){
		directorWorker = gameObject.AddComponent<DirectorWorker> ();
	}

	void FixedUpdate(){
		var pos = collisionPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		if(!target)
			target = Physics2D.OverlapCircle (pos, collisionRadius, collisionLayer);

		if (target) {
			if (timeElapsed == 0f) {
				OnCollect (target.gameObject);
				directorWorker.DoHideObject ("true");
			}

			timeElapsed += Time.deltaTime;
			if (timeElapsed >= destroyAfter) {
				OnDestroy ();
			}
		}
	}

	protected virtual void OnCollect(GameObject target){

	}

	protected virtual void OnDestroy(){
		Destroy (gameObject);
	}

	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;
		var pos = collisionPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, collisionRadius);
	}
}
