using System;
using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
	public GameObject target;
    public float damping = 1;
    public float lookAheadFactor = 1;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;
	public float repositioningFactorX = 1.2f;
	public float repositioningFactorY = .8f;
	public bool freezX, freezY;
	public float constX = 0f;
	public float constY = 0f;
	public bool disableLookdown;
	public bool isDoingLookDown;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

	private LedgeGrabbing ledgeGrabbingBehavoiur;
	private Duck duckBehavoiur;
	private InputState inputState;

    // Use this for initialization
    private void Start()
    {
		if (!target) {
			target = GameObject.FindGameObjectWithTag ("Player");
		}
		if (target.tag == "Player") {
			ledgeGrabbingBehavoiur = target.GetComponent<LedgeGrabbing> ();
			duckBehavoiur = target.GetComponent<Duck> ();
			inputState = target.GetComponent<InputState> ();
		}

		m_LastTargetPosition = target.transform.position;
        m_OffsetZ = (transform.position - target.transform.position).z;
        transform.parent = null;
    }

    // Update is called once per frame
    private void Update()
	{
		// only update lookahead pos if accelerating or changed direction
		if(!target) 
			return;

		float xMoveDelta = (target.transform.position - m_LastTargetPosition).x;

		bool updateLookAheadTarget = Mathf.Abs (xMoveDelta) > lookAheadMoveThreshold;

		if (updateLookAheadTarget) {
			m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign (xMoveDelta);
		} else {
			m_LookAheadPos = Vector3.MoveTowards (m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
		}
			
		Vector3 aheadTargetPos = Vector3.zero;
		//if (closeUp) {
		//	aheadTargetPos = new Vector3 (closeUp ? constX : target.position.x + repositioningFactorX , closeUp ? constY : target.position.y - repositioningFactorY, target.position.z) + m_LookAheadPos + Vector3.forward * m_OffsetZ;
		//} else if(!closeUp) {
		if (target.tag == "Player" && !disableLookdown && ((ledgeGrabbingBehavoiur.onLedgeGrabbingDetected || ledgeGrabbingBehavoiur.onLedgeTeetering) || duckBehavoiur.duckingDuration >= 1.7f && !duckBehavoiur.fourswalking)) {
			aheadTargetPos = new Vector3 (freezX ? constX + repositioningFactorX : target.transform.position.x + repositioningFactorX * ((float)inputState.direction), freezY ? constY + repositioningFactorY : target.transform.position.y - repositioningFactorY, target.transform.position.z) + m_LookAheadPos + Vector3.forward * m_OffsetZ;
			isDoingLookDown = true;

		} else {
			var dir = target.tag == "Player" ? (float)inputState.direction : 1;
			aheadTargetPos = new Vector3 (freezX ? constX + repositioningFactorX : target.transform.position.x + repositioningFactorX * dir, freezY ? constY + repositioningFactorY : target.transform.position.y + repositioningFactorY, target.transform.position.z) + m_LookAheadPos + Vector3.forward * m_OffsetZ;
			isDoingLookDown = false;
		}
		//	}

		Vector3 newPos = Vector3.SmoothDamp (transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

		transform.position = newPos;

		m_LastTargetPosition = target.transform.position;
	}
}

