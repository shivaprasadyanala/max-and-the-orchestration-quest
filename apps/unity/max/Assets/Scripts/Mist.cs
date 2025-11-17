using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour {

	public bool isOn = true;
	public Vector3 particlePosition;
	public Vector3 particleVelocity = Vector3.zero;

	public LayerMask collisionLayer;
	public Vector3 collisionCubeSize;
	private Color debugCollisionColor = Color.red;
	public Collider2D targetTrigger;

	private ParticleSystem myParticleSystem;
	private ParticleSystem.VelocityOverLifetimeModule vel;
	private ParticleSystem.ShapeModule shape;

	// Use this for initialization
	void Start () {
		// Get the system and the module(s).
		myParticleSystem = GetComponentInChildren<ParticleSystem>();
		vel = myParticleSystem.velocityOverLifetime;
		shape = myParticleSystem.shape;
	}
	
	// Update is called once per frame
	void Update () {
		myParticleSystem.transform.localPosition = particlePosition;

		shape.scale = collisionCubeSize;

		vel.xMultiplier = particleVelocity.x;
		vel.yMultiplier = particleVelocity.y;
		vel.zMultiplier = particleVelocity.z;

		//Debug.Log (vel.xMultiplier);
		if (!isOn)
			myParticleSystem.Stop ();
		else
			myParticleSystem.Play ();
	}

	void FixedUpdate () {
		var pos = Vector3.zero;
		pos.x += transform.position.x;
		pos.y += transform.position.y;
		targetTrigger = Physics2D.OverlapBox(pos, collisionCubeSize, 1, collisionLayer);
		if (targetTrigger) {			
			if(targetTrigger.tag.ToLower() == "wind"){
				Wind wind = targetTrigger.GetComponent<Wind> ();
				if (wind.isOn) {
					//Debug.Log (particleVelocity.x + " flow" + wind.flow);
					if (particleVelocity.x < wind.flow)
						particleVelocity.x += .05f;
					else if (particleVelocity.x > wind.flow)
						particleVelocity.x -= .05f;
						
					
				} else {
					if (particleVelocity.x > 0)
						particleVelocity.x -= .05f;
					else
						particleVelocity.x += .05f;
				}
			}

		} else if (!targetTrigger) {
			//triggerd = false;
		}
	}



	void OnDrawGizmos(){
		Gizmos.color = debugCollisionColor;

		//draw hit area
		var bpos = Vector3.zero;
		bpos.x += transform.position.x;
		bpos.y += transform.position.y;
		Gizmos.DrawWireCube (bpos, collisionCubeSize);
	}
}
