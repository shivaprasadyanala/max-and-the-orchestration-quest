using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyRay : MonoBehaviour {

	public LayerMask collisionLayer;
	public Vector2 collisionPosition = Vector2.zero;
	public float collisionRadius = .5f;
	public GameObject target;

	public Explode1 explosion; 
	private Explode1 clone;

	public float flySpeed = 2f;
	public Vector2 flyToPoint;

	private Collider2D targetHit;
	private float lifeTime;
	private Vector2 startPoint;
	private float furtherFactor = 3f;
	private Color debugCollisionColor = Color.yellow;

	// Use this for initialization
	void Start () {
		startPoint = transform.position;

		Debug.Log ("Spyray : " + target.tag);
		if(!target)
			target = GameObject.FindGameObjectWithTag ("Player");

		if(target) {
			//flyToPoint = player.transform.position;

			float r = AngleDir (transform.position, target.transform.position);

			flyToPoint = new Vector2 (target.transform.position.x + (furtherFactor-1f) * r, target.transform.position.y-furtherFactor);
		}
	}

	float AngleDir(Vector2  a, Vector2 b){
		//return -a.x  + a.y ;
		var direction = System.Math.Round (((a - b)).x, 2);
		if (direction > 0.23f)
			return -1f;
		else if (direction < -0.23f)
			return 1f;
		else if (direction > -.23f && direction < .23f) {			
			return 0f;
		}
		return 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		var pos = collisionPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		targetHit = Physics2D.OverlapCircle (pos, collisionRadius, collisionLayer);

		if (targetHit) {
			//spyray doesn't hurt the spy itself
			if (targetHit.tag != "Enemy-spy") {
				Debug.Log (targetHit.name + " " + targetHit.tag);
				Destroy (gameObject);

				if (explosion && !clone) {
					clone = Instantiate (explosion, transform.position, Quaternion.identity) as Explode1;
					clone.explosionPower = 50f;
				}		
			}
		}
			
		transform.position = Vector2.MoveTowards (new Vector2 (transform.position.x, transform.position.y), flyToPoint, flySpeed * Time.deltaTime);

		lifeTime += .01f;
		if (lifeTime > furtherFactor) {
			lifeTime = 0f;
			Destroy (gameObject);
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;
		Gizmos.DrawLine (startPoint, flyToPoint);
		Gizmos.DrawWireSphere (flyToPoint, .5f);
	}
}