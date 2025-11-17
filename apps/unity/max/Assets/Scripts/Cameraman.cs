using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour {

	public LayerMask collisionLayer;
	public Vector3 collisionCubeSize;
	private Color debugCollisionColor = Color.cyan;

	public GameObject newTarget;
	public float zoomSpeed = 2f;
	public float newZoomLevel = 2.7f;
	public float newRepositioningFactorX = 1.2f;
	public float newRepositioningFactorY = 0f;
	public bool cameraFreezX, cameraFreezY;
	public float theTimeScale = 1f;
	public float cancelAfter = 0f;
	public bool applyOnce;

	private bool triggered;
	private bool atCameramanZone;
	private float defaultCameraOrthographicSize;
	private Camera2DFollow camera2dFollow;
	private CameraZoom cameraZoom;
	private float defaultCameraRepositioningX;
	private float defaultCameraRepositioningY;
	private bool defaultCameraFreezX, defaultCameraFreezY;
	private GameObject defaultCameraTarget;
	private Collider2D target;
	public float timeElapsed = 0f;
	// Use this for initialization
	void Start () {
		defaultCameraOrthographicSize = Camera.main.orthographicSize;
		camera2dFollow = Camera.main.GetComponent<Camera2DFollow> ();
		cameraZoom = Camera.main.GetComponent<CameraZoom> ();
		defaultCameraRepositioningX = camera2dFollow.repositioningFactorX;
		defaultCameraRepositioningY = camera2dFollow.repositioningFactorY;
		defaultCameraTarget = camera2dFollow.target;
		defaultCameraFreezX = camera2dFollow.freezX;
		defaultCameraFreezY = camera2dFollow.freezY;
	}
	
	// Update is called once per frame
	void Update () {

		var pos = Vector3.zero;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		target = Physics2D.OverlapBox(pos, collisionCubeSize, 1, collisionLayer);

		if (target && !atCameramanZone && !triggered) {
			atCameramanZone = true;
			triggered = true;

			Time.timeScale = theTimeScale;

			if(newTarget) camera2dFollow.target = newTarget;
			camera2dFollow.freezX = cameraFreezX;
			camera2dFollow.freezY = cameraFreezY;
			camera2dFollow.constX = transform.position.x;
			camera2dFollow.constY = transform.position.y;
			camera2dFollow.repositioningFactorX = newRepositioningFactorX;
			camera2dFollow.repositioningFactorY = newRepositioningFactorY;

			if (cameraZoom) {
				cameraZoom.duration = zoomSpeed;
				cameraZoom.zoomLevel = newZoomLevel;
			}

		} else if (!target && atCameramanZone) {
			if(!applyOnce)
				atCameramanZone = false;
			if(cancelAfter == 0f)
				OffCameraman ();
		}

		if (triggered && cancelAfter != 0f && timeElapsed <= cancelAfter) {
			timeElapsed += Time.deltaTime;

			if (triggered) {
				if(newTarget)
					camera2dFollow.target = newTarget;
			}
		}

		if (triggered && timeElapsed >= cancelAfter && cancelAfter != 0f) {
			OffCameraman ();
		}
	}

	void OffCameraman(){
		Time.timeScale = 1f;

		if (cameraZoom) {
			cameraZoom.duration = zoomSpeed;
			cameraZoom.zoomLevel = defaultCameraOrthographicSize;
		}
		camera2dFollow.repositioningFactorX = defaultCameraRepositioningX;
		camera2dFollow.repositioningFactorY = defaultCameraRepositioningY;
		camera2dFollow.freezX = defaultCameraFreezX;
		camera2dFollow.freezY = defaultCameraFreezY;
		if (defaultCameraTarget)
			camera2dFollow.target = defaultCameraTarget;

		if (triggered && !applyOnce) {
			triggered = false;
			timeElapsed = 0f;

		} else if (triggered && applyOnce && cancelAfter > 0f) {
			//newTarget = null;
			//cancelAfter = 0f;
			triggered = false;
			atCameramanZone = true;
			timeElapsed = 0f;
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;
		//draw hit area
		var bpos = Vector3.zero;
		bpos.x += transform.position.x;
		bpos.y += transform.position.y;
		Gizmos.DrawWireCube (bpos, collisionCubeSize);

		//draw the drag handle
		Color color  = debugCollisionColor;
		color.a = .4f;
		Gizmos.color = color;
		Gizmos.DrawCube (bpos, new Vector3(.3f, .3f, 0));
	}

	void OnDisable(){
		applyOnce = false;
		atCameramanZone = false;
		OffCameraman ();
	}
}
