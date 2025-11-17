using UnityEngine;
using System.Collections;

public class Meter : MonoBehaviour {

	public float battery = 10;
	public float maxBattery = 10;
	public float batteryDechargeRate = 1f;
	//public Texture2D bgTexture;
	public Texture2D batteryTexture0;
	public Texture2D batteryTexture1;
	public Texture2D batteryTexture2;
	public Texture2D batteryTexture3;
	public Texture2D batteryTexture4;
	//public int iconWidth = 32;
	public Vector2 batteryOffset = new Vector2(10, 10);

	private Equip equipBehavior;

	// Use this for initialization
	void Start () {
		equipBehavior = GameObject.FindObjectOfType<Equip> ();
	}

	void OnGUI(){
		var percent = Mathf.Clamp01 (battery / maxBattery);

		if (!equipBehavior)
			percent = 0;

		DrawMeter (batteryOffset.x, batteryOffset.y, percent);
	}

	void DrawMeter(float x, float y, float percent){
		Texture2D texture = batteryTexture0;

		//Debug.Log (percent);
		if (percent <= 0) {
			texture = batteryTexture0;
		} else if (percent > 0 && percent < .2) {//blinking
			if (Time.time % 2 > 1) {
				texture = batteryTexture1;
			} else {
				texture = batteryTexture0;
			}
		} else if (percent > .2 && percent < .4) {
			texture = batteryTexture1;
		} else if (percent > .4 && percent < .6) {
			texture = batteryTexture2;
		} else if (percent > .6 && percent < .8) {
			texture = batteryTexture3;
		} else if (percent > .8) {
			texture = batteryTexture4;
		}

		var bgW = texture.width;
		var bgH = texture.height;

		//GUI.DrawTexture (new Rect (x, y, bgW, bgH), background);

		//var nW = ((bgW - iconWidth) * percent) + iconWidth;

		GUI.BeginGroup (ResizeGUI(new Rect (x, y, bgW, bgH)));
		GUI.DrawTexture (ResizeGUI(new Rect (0, 0, bgW, bgH)), texture);
		GUI.EndGroup ();
	}

	Rect ResizeGUI(Rect _rect)
	{
		CameraPixelPerfect ppc = Camera.main.GetComponent<CameraPixelPerfect> ();

		float FilScreenWidth = _rect.width / ppc.nativeResolution.x;
		float rectWidth = FilScreenWidth * Screen.width;
		float FilScreenHeight = _rect.height / ppc.nativeResolution.y;
		float rectHeight = FilScreenHeight * Screen.height;
		float rectX = (_rect.x / ppc.nativeResolution.x) * Screen.width;
		float rectY = (_rect.y / ppc.nativeResolution.y) * Screen.height;

		return new Rect (rectX, rectY, rectWidth, rectHeight);
	}

	// Update is called once per frame
	void Update () {
		if (equipBehavior.currentItem == 0)
			return;

		if (battery > 0) {
			battery -= Time.deltaTime * batteryDechargeRate;

		} else {
			equipBehavior.currentItem = 0;
		}
	}
}
