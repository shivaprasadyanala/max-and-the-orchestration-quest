using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Collectable {

	public float BatteryAmount = 40f;

	override protected void OnCollect(GameObject target){

		var ap = GetComponent<AudioPlayer> ();
		if (ap) {
			ap.Audio_Play_Clip ();
			Debug.Log ("battery picked up!!");
		}

		var meter = GameObject.FindObjectOfType<Meter> ();
		if(meter){
			meter.battery += BatteryAmount;
		}
	}
}
