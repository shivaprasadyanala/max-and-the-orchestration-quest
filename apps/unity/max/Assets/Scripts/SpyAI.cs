using UnityEngine;
using System.Collections;

public class SpyAI : MonoBehaviour {

	public float spyHealth = 100f;

	public enum DetectedObject
	{
		Nothing=0,
		Target,
		TargetIgnoring,
		PushBlock,
	}
	public enum SpyMode
	{
		Searching=0,
		Lurking,
		Dead,
	}

	public float followSpeed;
	public float ingnoringSpeed = .004f;

	public LayerMask layerMask;
	public GameObject target;
	public SpyMode spyMode;
	public Collider2D[] scannedObjects;
	private DetectedObject detectedObject;

	public Vector2 bottomPosition = Vector2.zero;
	public Vector2 rightPosition = Vector2.zero;
	public Vector2 leftPosition = Vector2.zero;
	public float collisionRadius = .1f;
	public Color debugCollisionColor = Color.red;
	public float collisionLeftAndRightRadius = .1f;

	//Classed And Components
	private Animator animator;
	private FloatEffect floatEffect;
	private Transform myTransform;
	private MoveAtoB moveAtoB;
	private Rigidbody2D scannedObjectRigidbody2D;
	private CircleCollider2D circleCollider2D;
	private ObjectFaceDirection spyFaceDirection;

	private float defaultRadius = 0f;
	private float defaultCollisionY = 0f;

	private Transform blockTransform;

	private float seenTime=0f;
	public SpyRay spyRay;
	private SpyRay rayclone;
	public Smoke smoke; 
	private Smoke smokeclone;
	public Light myLight;
	private AudioPlayer audioPlayer;
	// Use this for initialization
	void Awake() {
		
	}

	void Start () {
		myTransform = transform;
		animator = GetComponent<Animator> ();
		circleCollider2D = GetComponent<CircleCollider2D> ();
		moveAtoB = GetComponent<MoveAtoB>();
		floatEffect = GetComponent<FloatEffect> ();
		spyFaceDirection = GetComponent<ObjectFaceDirection> ();
		myLight = GetComponentInChildren<Light> ();
		audioPlayer = GetComponent<AudioPlayer> ();

		defaultRadius = collisionRadius;
		defaultCollisionY = bottomPosition.y;

		detectedObject = DetectedObject.Nothing;
	}

	void Update(){
		if (spyHealth <= 0) {
			spyMode = SpyMode.Dead;
			this.enabled = false;
			floatEffect.enabled = false;
			moveAtoB.enabled = false;
			spyFaceDirection.enabled = false;


			if (smoke && !smokeclone) {				
				smokeclone = Instantiate (smoke, transform) as Smoke;
				smokeclone.smokeScale = .5f;
			}

			animator.SetInteger ("AnimState", 5);
			transform.Rotate (0f, 0f, 15f);
			//GetComponent<Rigidbody2D> ().mass = 8;
			GetComponent<Rigidbody2D> ().linearDamping = 0f;
			GetComponent<Rigidbody2D> ().gravityScale = 1f;
			myLight.enabled = false;
		}

		if (spyMode == SpyMode.Lurking) {
			animator.SetInteger ("AnimState", 6);
			circleCollider2D.enabled = false;
			moveAtoB.enabled = false;
			floatEffect.enabled = false;
			myLight.enabled = false;

		} else if (spyMode == SpyMode.Searching) {			
			circleCollider2D.enabled = true;
			if (detectedObject == DetectedObject.Nothing) {
				animator.SetInteger ("AnimState", 0);
				moveAtoB.enabled = true;
				moveAtoB.isMoving = true;
				floatEffect.enabled = true;
				myLight.enabled = true;
				myLight.color = Color.green;
			}
		}
	}
		
	void FixedUpdate(){
		
		var pos = bottomPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		scannedObjects = Physics2D.OverlapCircleAll (pos, collisionRadius, layerMask);


//		pos = spyFaceDirection.direction == SpyFaceDirection.SpyDirections.Right ? rightPosition : leftPosition;
//		pos.x += transform.position.x;
//		pos.y += transform.position.y;		
//		//onFense = Physics2D.OverlapCircle (pos, collisionRadius, collisionLayer);
//		pushBlock = Physics2D.OverlapCircle (pos, collisionLeftAndRightRadius, collisionLayer2);

//		if (pushBlock) {
//			if (pushBlock.gameObject.tag == "Block" && detectedObject != DetectedObject.PushBlock) {
//
//				blockTransform = pushBlock.transform;
//				detectedObject = DetectedObject.PushBlock;
//
//				spySearching.enabled = false;
//				floatEffect.enabled = false;
//			}
//		}
//		if (detectedObject == DetectedObject.PushBlock) {
//			var pushBluckPos = new Vector3(blockTransform.position.x+.1f, blockTransform.position.y+1f, blockTransform.position.z);
//			//myTransform.position += (pushBluckPos - myTransform.position) * followSpeed * Time.deltaTime;
//			myTransform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), pushBluckPos, followSpeed * Time.deltaTime);
//
//			var direction = System.Math.Round(((pushBluckPos - myTransform.position)).x, 2);
//			if (direction > -.06f && direction < .06f) {
//				spySearching.enabled = true;
//				floatEffect.enabled = true;
//				//floatEffect.startY = transform.position.y;
//
//				detectedObject = DetectedObject.Nothing;
//			}
//		}


		if (scannedObjects != null) {
			
			Collider2D scannedObject = IsTargetAtVisibleRange (target);
			if (scannedObject && detectedObject != DetectedObject.Target) {
				scannedObjectRigidbody2D = scannedObject.GetComponent<Rigidbody2D> ();

				if (scannedObjectRigidbody2D.linearVelocity != Vector2.zero) {
				
					RestoreDefaultCollisionSettings ();
					moveAtoB.enabled = false;
					spyFaceDirection.isManual = true;
					floatEffect.enabled = false;
					spyMode = SpyMode.Searching;
					myLight.enabled = true;
					myLight.color = Color.red;

					detectedObject = DetectedObject.Target;
				}

			} else if (!scannedObject) {
				if (defaultRadius != collisionRadius) {
					RestoreDefaultCollisionSettings ();
				}	
				if (detectedObject == DetectedObject.TargetIgnoring) {
					animator.SetInteger ("AnimState", 0);
					moveAtoB.enabled = true;
					spyFaceDirection.isManual = false;
					floatEffect.enabled = true;
					seenTime = 0.0f;

					detectedObject = DetectedObject.Nothing;
				}
			}
		}

		if (detectedObject == DetectedObject.Target) {
			
			//Move Towards Target
			#region
			var targetPos = new Vector3 (target.transform.position.x, target.transform.position.y + 1.4f, target.transform.position.z);
			myTransform.position += (targetPos - myTransform.position) * followSpeed * Time.deltaTime;
			var direction = System.Math.Round (((targetPos - myTransform.position)).x, 2);
			//Debug.Log(direction);
			if (direction > 0.23f) {					
				spyFaceDirection.direction = ObjectFaceDirection.FaceDirections.Right;

				seenTime += Time.deltaTime;
				if (seenTime <= 4f) {
					animator.SetInteger ("AnimState", 1);

				} else if (seenTime > 4f && seenTime < 6.5f) {
					animator.SetInteger ("AnimState", 3);
					audioPlayer.Audio_Play_Clip();

				} else if (seenTime >= 6.5f) {
					seenTime = 7f;
					Fire (target);
				}

			} else if (direction < -0.23f) {
				spyFaceDirection.direction = ObjectFaceDirection.FaceDirections.Left;

				seenTime += Time.deltaTime;
				if (seenTime <= 4f) {
					animator.SetInteger ("AnimState", 1);

				} else if (seenTime > 4f && seenTime < 6.5f) {
					animator.SetInteger ("AnimState", 3);
					audioPlayer.Audio_Play_Clip();

				} else if (seenTime >= 6.5f) {
					seenTime = 7f;
					Fire (target);
				}

			} else if (direction > -.23f && direction < .23f) {					
				animator.SetInteger ("AnimState", 2);

				seenTime += Time.deltaTime;
				if (seenTime <= 4f) {
					animator.SetInteger ("AnimState", 2);

				} else if (seenTime > 4f && seenTime < 6.5f) {
					animator.SetInteger ("AnimState", 4);
					audioPlayer.Audio_Play_Clip();

				} else if (seenTime >= 6.5f) {
					seenTime = 7f;
					Fire (target);
				}
			}
			#endregion

			//if target freezed in the place.
			if ((scannedObjectRigidbody2D.linearVelocity == Vector2.zero) && direction == 0) {
				detectedObject = DetectedObject.TargetIgnoring;

				myLight.color = Color.white;

				audioPlayer.Audio_Pause_Clip ();
			}

		} else if (detectedObject == DetectedObject.TargetIgnoring) {
			collisionRadius -= ingnoringSpeed;
			bottomPosition.y += ingnoringSpeed;
		}
	}

//	private void Audio_Play_Spyray(){
//		var ap = GetComponent<AudioPlayer> ();
//		if (ap) {
//			ap.Audio_Play_Clip ();
//		}
//	}

	private Collider2D IsTargetAtVisibleRange(GameObject key){
		foreach (Collider2D scannedObject in scannedObjects) {
			if (scannedObject.transform == key.transform) {
				return scannedObject;
			}
		}
		return null;
	}

	public void Fire(GameObject target){
		if (!rayclone) {
			Vector2 spyraySpawnPoint = new Vector2 (transform.position.x, transform.position.y);
			rayclone = Instantiate (spyRay, spyraySpawnPoint, Quaternion.identity) as SpyRay;
			rayclone.target = target;
			seenTime = 0f;
		}

	}

	void RestoreDefaultCollisionSettings(){
		collisionRadius = defaultRadius;
		bottomPosition.y = defaultCollisionY;
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;

		if (targetTag == "Deadly-saw"){			

			/*if (spark) {
				clone = Instantiate (spark, transform.position, Quaternion.identity) as BloodSpray;
			}*/

			spyHealth -= 40;

		}
	}
	void OnCollisionEnter2D(Collision2D target){
		DoReaction (target, null);
	}
	void OnTriggerEnter2D(Collider2D target){
		DoReaction (null, target);
	}


	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;

		var pos = bottomPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, collisionRadius);

		pos = rightPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, collisionLeftAndRightRadius);

		pos = leftPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, collisionLeftAndRightRadius);
	}
}