using UnityEngine;
using System.Collections;

public class FlashLight : Collectable {
	
	public int itemID = 1;

	override protected void OnCollect(GameObject target){

		var ap = GetComponent<AudioPlayer> ();
		if (ap) {
			ap.Audio_Play_Clip ();
			Debug.Log ("flashhhhhhhhhhh");
		}

		var meter = GameObject.FindObjectOfType<Meter> ();
		if (meter) {
			meter.enabled = true;
			var equipBehavior = target.GetComponent<Equip> ();
			if (equipBehavior) {
				equipBehavior.currentItem = itemID;
			}
		}
	}
}
