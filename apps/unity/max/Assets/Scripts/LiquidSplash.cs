using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSplash : MonoBehaviour {
	private ParticleSystem ps;


	public void Start() 
	{
		ps = GetComponent<ParticleSystem>();
	}

	public void Update() 
	{
		if(ps){

			var ap = GetComponent<AudioPlayer> ();
							
				if (ap && !ap.isPlaying) {
					ap.Audio_Play_Clip ();
				}

			if(!ps.IsAlive())
				Destroy(gameObject);
		}

	}
}
