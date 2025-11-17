using System.Collections;
using UnityEngine;

public class Liquid : MonoBehaviour {

	public enum LiquidState{
		Fixed=0, 
		Draining,
		Overflowing
	}

	public enum LiquidTypes {
		Water=0,
		Acid,
		Lava
	}
	public LayerMask collisionLayer;
	public Vector3 collisionCubeSize;

	public LiquidTypes liquidType;
	public float flowDirection;
	public LiquidState liquidState = LiquidState.Fixed;
	public float drainSpeed = .003f;
	public LiquidSplash splash;
    public float liquidSurfaceOffset = -.3f;

    private float liquidDensity;
	//private AnimatedTexture animatedTexture;
	private BuoyancyEffector2D buoyancyEffector2d;
	private BoxCollider2D boxCollider2d;
	private float flow;
    private float defaultPlayerGravity;//, defaultSurfaceLevel, defaultLiquidHeight;
	private int defaultRendererSortingOrder = 0;
    public float maxLiquidHeight;

	private Color debugCollisionColor = Color.red;
	private Collider2D targetTrigger;
	private MeshRenderer _renderer;    

    //	private RaycastHit2D[] hits;
    // Use this for initialization
    void Start () {

		_renderer = GetComponent<MeshRenderer> ();
		//animatedTexture = GetComponent<AnimatedTexture> ();
		buoyancyEffector2d = GetComponent<BuoyancyEffector2D> ();

		if (liquidType == LiquidTypes.Water) {
			tag = "Liquid-Water";
			liquidDensity = 10;
			flow = flowDirection;

		} else if (liquidType == LiquidTypes.Acid) {
			tag = "Liquid-Acid";
			liquidDensity = 20;
			flowDirection = -.07f;
			flow = 0f;	
			StartCoroutine (UpdateFlow (.01f));

		} else if (liquidType == LiquidTypes.Lava) {
			tag = "Liquid-Lava";
			liquidDensity = 20;
			flowDirection = .02f;
			flow = 0f;	
			StartCoroutine (UpdateFlow (.01f));
		}
			
		// buoyancyEffector2d.linearDrag = liquidDensity;
		buoyancyEffector2d.angularDamping = liquidDensity;

		if(_renderer && maxLiquidHeight == 0)
			maxLiquidHeight = _renderer.transform.localScale.y;

		var target = GameObject.FindGameObjectWithTag ("Player");
		if(target)
			defaultPlayerGravity =  target.GetComponent<Rigidbody2D> ().gravityScale;

		//defaultSurfaceLevel = buoyancyEffector2d.surfaceLevel;
  //      if(_renderer)
		//    defaultLiquidHeight = _renderer.transform.localScale.y;
	}

	void FixedUpdate(){

        //buoyancyEffector2d.surfaceLevel = (.38f / transform.lossyScale.y);

        // Preventer #1
        if (_renderer.transform.localScale.y > maxLiquidHeight && liquidState == LiquidState.Overflowing) {
            _renderer.transform.localScale = new Vector2 (_renderer.transform.localScale.x, maxLiquidHeight);
			liquidState = LiquidState.Fixed;

            // Preventer #2
		} else if (_renderer.transform.localScale.y < .01f && liquidState == LiquidState.Draining) {
            _renderer.transform.localScale = new Vector2 (_renderer.transform.localScale.x, .01f);
			liquidState = LiquidState.Fixed;
		}

		if (liquidState == LiquidState.Draining || liquidState == LiquidState.Overflowing) { 
			//transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y + (liquidState == LiquidState.Draining ? -drainSpeed : drainSpeed), transform.localScale.z);
			transform.position = new Vector3 (transform.position.x, transform.position.y + (liquidState == LiquidState.Draining ? -(drainSpeed/2) : drainSpeed/2), transform.position.z);
            //Debug.Log(spriteRenderer.size.y);
            transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y + (liquidState == LiquidState.Draining ? -drainSpeed : drainSpeed));


            GetComponent<BoxCollider2D>().offset = new Vector2(0, liquidSurfaceOffset / transform.lossyScale.y);
            GetComponent<BoxCollider2D>().size = new Vector3(_renderer.bounds.size.x / transform.lossyScale.x,
                                         _renderer.bounds.size.y / transform.lossyScale.y,
                                         _renderer.bounds.size.z / transform.lossyScale.z);

            

            //buoyancyEffector2d.surfaceLevel = ((_renderer.transform.localScale.y * defaultSurfaceLevel) / defaultLiquidHeight);
        }
			
		/////////////////////////////////////////////////////////////////////////////
		var pos = Vector3.zero;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		targetTrigger = Physics2D.OverlapBox (pos, collisionCubeSize, 1, collisionLayer);
		//Debug.Log (targetTrigger);
		if (targetTrigger) {
			//triggerd = true;
			if (targetTrigger.tag.ToLower () == "wind") {
				Wind wind = targetTrigger.GetComponent<Wind> ();
				if (wind.isOn) {
					if (wind.flow > 0)
						flowDirection = .05f;
					else if (wind.flow < 0)
						flowDirection = -.05f;
				} else {
					flowDirection = 0f;
				}
			}

		} else if (!targetTrigger) {
			//triggerd = false;
		}

//		Debug.DrawLine (new Vector3 (pos.x - collisionCubeSize.x / 2, pos.y + collisionCubeSize.y / 2 + .05f, pos.z), new Vector3 (pos.x + collisionCubeSize.x / 2, pos.y + collisionCubeSize.y / 2 + .05f, pos.z), Color.green);
//		hits = Physics2D.LinecastAll (new Vector3 (pos.x - collisionCubeSize.x / 2, pos.y + collisionCubeSize.y / 2 + .05f, pos.z), new Vector3 (pos.x + collisionCubeSize.x / 2, pos.y + collisionCubeSize.y / 2 + .05f, pos.z), collisionLayer);
//		if (hits != null) {
//			
//			//Debug.Log ("hit : " + hits[0].collider.gameObject.tag);
//
//		}
			
	}
	
	// Update is called once per frame
	void Update () {

		// Liquid Current and Flow
		if (liquidType == LiquidTypes.Water || liquidType == LiquidTypes.Acid) {			
			//animatedTexture.speed = new Vector2 (flow * -1, 0);
			buoyancyEffector2d.flowAngle = flow * 50;
			buoyancyEffector2d.flowMagnitude = flow * 50;

			if (flowDirection != flow)
				flow = flowDirection;

		} else if (liquidType == LiquidTypes.Lava) {
			//animatedTexture.speed = new Vector2 (flow * -1, 0);

		}
			
	}

	IEnumerator UpdateFlow(float dir){
		
		yield return new WaitForSeconds (1f);
		
		flow = dir;
		StartCoroutine (UpdateFlow (dir * -1));
		//Debug.Log("after " + flow);
	}

	void DoSplash(GameObject target){
//		var hit = System.Array.Find (hits, p => p.collider.gameObject == target.gameObject);
//		Debug.Log (hit.collider.gameObject.tag);
		if (splash) {
			Instantiate (splash, new Vector3(target.transform.position.x, target.transform.position.y - .5f, 0f) , Quaternion.Euler(-90f, 0f, 0f));
			//clone.transform.parent = transform;
		}
	}

	void DoReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;	

		DoSplash (target);

		if (target.tag == "Player") {
			
			target.GetComponent<Rigidbody2D> ().linearVelocity = new Vector2 (target.GetComponent<Rigidbody2D> ().linearVelocity.x, target.GetComponent<Rigidbody2D> ().linearVelocity.y / 1.5f);
			target.GetComponent<Rigidbody2D> ().useAutoMass = false;
			target.GetComponent<Rigidbody2D> ().mass = .5f;
			target.GetComponent<Rigidbody2D> ().gravityScale = .1f;

			if (defaultRendererSortingOrder == 0) {
				// decrease the player render order 1 less than the water render order
				defaultRendererSortingOrder = target.GetComponent<SpriteRenderer> ().sortingOrder;
			}
			//Debug.Log (defaultRendererSortingOrder);
			//target.GetComponent<MeshRenderer> ().sortingOrder = GetComponent<SpriteRenderer> ().sortingOrder - 1;

		}
	}
	void DoExitReaction(Collision2D collisionTarget, Collider2D colliderTarget){

		GameObject target = collisionTarget != null ? collisionTarget.gameObject : colliderTarget.gameObject;
		//Vector2 targetPosision = collisionTarget != null ? collisionTarget.transform.position : colliderTarget.transform.position;
		//Rigidbody2D targetBody2d = collisionTarget != null ? collisionTarget.gameObject.GetComponent<Rigidbody2D> () : colliderTarget.gameObject.GetComponent<Rigidbody2D> ();
		//string targetTag = collisionTarget != null ? collisionTarget.gameObject.tag : colliderTarget.gameObject.tag;


		DoSplash (target);
		//Debug.Log (defaultRendererSortingOrder);
		if (target.tag == "Player") {			
			target.GetComponent<SpriteRenderer> ().sortingOrder = defaultRendererSortingOrder;
			target.GetComponent<Rigidbody2D> ().useAutoMass = true;
			target.GetComponent<Rigidbody2D> ().gravityScale = defaultPlayerGravity;
		}
	}
	void OnCollisionEnter2D(Collision2D target){
		DoReaction (target, null);
	}
	void OnTriggerEnter2D(Collider2D target){
		DoReaction (null, target);
	}
	void OnCollisionExit2D(Collision2D target){
		DoExitReaction (target, null);
	}
	void OnTriggerExit2D(Collider2D target){
		DoExitReaction (null, target);
	}

	void OnDrawGizmos(){

		boxCollider2d = GetComponent<BoxCollider2D> ();
		collisionCubeSize = boxCollider2d.bounds.size;
        
		Gizmos.color = debugCollisionColor;

        //draw hit area
        var bpos = boxCollider2d.bounds.center;
        //bpos.x += boxCollider2d.offset.x;
        //bpos.y += boxCollider2d.offset.y;
		Gizmos.DrawWireCube (bpos, collisionCubeSize);
	}
}
