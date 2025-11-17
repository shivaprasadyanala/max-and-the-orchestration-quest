using UnityEngine;
using System.Collections;

public class CollisionState : MonoBehaviour {

	public LayerMask groundLayer;
	public LayerMask pushBlockLayer;
	public LayerMask ropeLayer;
	public LayerMask ladderLayer;
	public LayerMask ledgeLayer;
	public LayerMask fenseLayer;
	public LayerMask environmentLayer;

	public Collider2D standing;
	public Collider2D ceiling;
	//public Collider2D groundInBehind;
	public Collider2D onPushBlock;
	public Collider2D onRope;
	public Collider2D onLadder;
	public Collider2D onLadder_playerHead;
	public Collider2D onLadder_playerBottom;
	public Collider2D onLedgeGrabbing;
	public Collider2D onLedgeStanding;
	public Collider2D onFense;
	public Collider2D rightObject;
	public Collider2D leftObject;
	public Collider2D onEnvironmentElement;
	public Collider2D onEnvironmentElement_playerHead;
	public Collider2D bottomFrontObject;
	public Collider2D bottomBackObject;

	public Vector2 bottomPosition = Vector2.zero;
	public Vector2 bottomRightPosition = Vector2.zero;
	public Vector2 bottomLeftPosition = Vector2.zero;
	public Vector2 rightPosition = Vector2.zero;
	public Vector2 leftPosition = Vector2.zero;
	public Vector2 topRightPosition = Vector3.zero;
	public Vector2 topLeftPosition = Vector3.zero;
	public Vector2 topTopPosition = Vector3.zero;
	public Vector2 bottomBottomPosition = Vector2.zero;
	public Vector3 linesSize = Vector3.zero;
	public Vector3 rectsSize = Vector3.zero;
	public float bottomRadius = .1f;
	public bool isTopRightRopePositionCollided = false;
	public bool isTopRightLedgePositionCollided = false;
	public Color debugCollisionColor = Color.red;

	private InputState inputState;

	// Use this for initialization
	void Awake () {
		inputState = GetComponent<InputState> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){

		var pos = bottomPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		standing = Physics2D.OverlapCircle (pos, bottomRadius, groundLayer | pushBlockLayer);
		onLadder = Physics2D.OverlapCircle (pos, bottomRadius, ladderLayer);
		onLedgeStanding = Physics2D.OverlapCircle (pos, bottomRadius, ledgeLayer);
		onEnvironmentElement = Physics2D.OverlapCircle (pos, bottomRadius, environmentLayer);
		//onSlippyGound = Physics2D.OverlapCircle (pos, bottomRadius, slippyGoundLayer);

		// bottom frony/bottom back object
		pos = inputState.direction == Directions.Right ? bottomRightPosition : bottomLeftPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		bottomFrontObject = Physics2D.OverlapCircle (pos, bottomRadius / 3f, groundLayer | pushBlockLayer);
		///
		pos = inputState.direction == Directions.Right ? bottomLeftPosition : bottomRightPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		bottomBackObject = Physics2D.OverlapCircle (pos, bottomRadius / 3f, groundLayer | pushBlockLayer);

		//left / right object
		pos = rightPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		rightObject = Physics2D.OverlapBox (pos, rectsSize, 1, groundLayer | pushBlockLayer);
		///
		pos = leftPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		leftObject = Physics2D.OverlapBox (pos, rectsSize, 1, groundLayer | pushBlockLayer);

		pos = inputState.direction == Directions.Right ? rightPosition : leftPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		onFense = Physics2D.OverlapBox (pos, rectsSize, 1, fenseLayer);
		onPushBlock = Physics2D.OverlapBox (pos, rectsSize, 1, pushBlockLayer);
		//onSideGround = Physics2D.OverlapBox (pos, rectsSize, 1, groundLayer | pushBlockLayer);

		pos = topRightPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		onRope = Physics2D.OverlapBox (pos, linesSize, 1, ropeLayer);
		isTopRightRopePositionCollided = true;
		if (!onRope) {
			pos = topLeftPosition;
			pos.x += transform.position.x;
			pos.y += transform.position.y;
			onRope = Physics2D.OverlapBox (pos, linesSize, 1, ropeLayer);
			isTopRightRopePositionCollided = false;
		}
		if ((isTopRightRopePositionCollided && inputState.direction == Directions.Left) ||
		    (!isTopRightRopePositionCollided && inputState.direction == Directions.Right)) {
			onRope = null;
		}

		pos = topRightPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		onLedgeGrabbing = Physics2D.OverlapBox (pos, linesSize, 1, ledgeLayer);
		isTopRightLedgePositionCollided = true;
		if (!onLedgeGrabbing) {
			pos = topLeftPosition;
			pos.x += transform.position.x;
			pos.y += transform.position.y;
			onLedgeGrabbing = Physics2D.OverlapBox (pos, linesSize, 1, ledgeLayer);
			isTopRightLedgePositionCollided = false;
		}
		if ((isTopRightLedgePositionCollided && inputState.direction == Directions.Left) ||
		    (!isTopRightLedgePositionCollided && inputState.direction == Directions.Right)) {
			onLedgeGrabbing = null;
		}
			
		//ceiling
		pos = topTopPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		ceiling = Physics2D.OverlapBox (pos, new Vector3 (linesSize.y, linesSize.x, linesSize.z), 1, groundLayer | 1 << LayerMask.NameToLayer("DetailObjUncollideWithSolid"));
		onEnvironmentElement_playerHead = Physics2D.OverlapBox (pos, new Vector3 (linesSize.y, linesSize.x, linesSize.z), 1, environmentLayer);
		onLadder_playerHead = Physics2D.OverlapBox (pos, new Vector3 (linesSize.y, linesSize.x, linesSize.z), 1, ladderLayer);

		//bottom bottom
		pos = bottomBottomPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		onLadder_playerBottom = Physics2D.OverlapBox (pos, new Vector3 (linesSize.y, linesSize.x/2f, linesSize.z), 1, ladderLayer);
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){
		//GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;
	}

	void OnCollisionEnter2D(Collision2D target){
		DoReaction (target, null);
	}
	void OnTriggerEnter2D(Collider2D target){
		DoReaction (null, target);
	}
	void OnCollisionStay2D(Collision2D target){
		//DoReaction (target, null);
	}




	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;

		//draw the circle
		var pos = bottomPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, bottomRadius);

		pos = bottomRightPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, bottomRadius/3f);

		pos = bottomLeftPosition;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		Gizmos.DrawWireSphere (pos, bottomRadius/3f);

		//draw the rects
		var rectpos = rightPosition;
		rectpos.x += transform.position.x;
		rectpos.y += transform.position.y;
		Gizmos.DrawWireCube (rectpos, rectsSize);

		rectpos = leftPosition;
		rectpos.x += transform.position.x;
		rectpos.y += transform.position.y;
		Gizmos.DrawWireCube (rectpos, rectsSize);

		//draw the lines
		var topPos = topRightPosition;
		topPos.x += transform.position.x;
		topPos.y += transform.position.y;
		Gizmos.DrawWireCube (topPos, linesSize);

		topPos = topLeftPosition;
		topPos.x += transform.position.x;
		topPos.y += transform.position.y;
		Gizmos.DrawWireCube (topPos, linesSize);

		topPos = topTopPosition;
		topPos.x += transform.position.x;
		topPos.y += transform.position.y;
		Gizmos.DrawWireCube (topPos, new Vector3(linesSize.y, linesSize.x, linesSize.z));

		topPos = bottomBottomPosition;
		topPos.x += transform.position.x;
		topPos.y += transform.position.y;
		Gizmos.DrawWireCube (topPos, new Vector3(linesSize.y, linesSize.x/2f, linesSize.z));
	}
}
