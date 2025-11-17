using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class TextPlayer : MonoBehaviour {

	public string text;
	public float displayDuration = 3f;
	public float topOffset = 50.0f;
	public bool isOn;
	public GUISkin guiSkin;

	private Transform target;
	private XmlDocument xmlDoc;
	//private TextAsset textAsset;
	private XmlNodeList xmlNode;
	private float timeElapsed = 0f;

	// Use this for initialization
	void Awake () {
		xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		//textAsset = (TextAsset)Resources.Load("Subtitles/" + CrossSceneInfo.Instance.languageSelected.ToString().ToLower(), typeof(TextAsset));
		//textAsset = CrossSceneInfo.Instance.subtitleAsset;
		xmlDoc.LoadXml(CrossSceneInfo.Instance.subtitleAsset.text);

		if (!target) {
			target = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if (target.tag == "Player" && target.GetComponent<PlayerPhysics> ().isDead) {
//			isOn = false;
//		}

		if (isOn && displayDuration > 0f) {
			
			timeElapsed += Time.deltaTime;

			if (timeElapsed >= displayDuration) {
				timeElapsed = 0f;
				isOn = false;
			}
		}
	}

	public string Text_Find_Item(string index){
		try {
			xmlNode = xmlDoc.SelectNodes ("/subtitles/sub[@index='" + index + "']/text");
			//Debug.Log("TextPlayer] xmlNode loaded.");
			text = xmlNode.Item (UnityEngine.Random.Range (0, xmlNode.Count)).InnerText;
			//Debug.Log("TextPlayer] text loaded.");

			return text;

		} catch (Exception ex) {
			Debug.Log (ex.Message);
		}

		return null;
	}		

	public void Text_Display_Item(string index, string duration){
		displayDuration = float.Parse(duration);
		Text_Find_Item (index);
		isOn = true;
	}
		
	public void Text_Hide_Item(){
		isOn = false;
	}

	public int Text_Display_Text(string txt, string duration){
		int randomNumber = 0;

		try {
			if (txt.ToLower ().Contains ("<")) {
				//Advs/adv<88,5>
				int def = txt.LastIndexOf (">") - txt.LastIndexOf ("<") + 1;
				//Debug.Log("def1 = " + def);
				string sub = txt.Substring (txt.LastIndexOf ("<"), def).Trim ();
				//Debug.Log("sub = " + sub);

				def = sub.LastIndexOf (",") - sub.LastIndexOf ("<") - 1;
				//Debug.Log("def1 = " + def);
				int num1 = int.Parse (sub.Substring (sub.LastIndexOf ("<") + 1, def).Trim ());
				//Debug.Log("num1 = " + num1);

				def = sub.LastIndexOf (">") - sub.LastIndexOf (",") - 1;
				//Debug.Log("def2 = " + def);
				int num2 = int.Parse (sub.Substring (sub.LastIndexOf (",") + 1, def).Trim ());
				//Debug.Log("num2 = " + num2);

				num1 = UnityEngine.Random.Range (num1, num2);
				//Replace 0 with "1" and 9 with "8"
				num1 = int.Parse(num1.ToString().Replace("0", "1"));
				randomNumber = int.Parse(num1.ToString().Replace("9", "8"));
				txt = txt.Replace (sub, randomNumber.ToString ());
			}

			Debug.Log ("TextPlayer] random number is:" + txt);

			//Debug.Log ("picture name is : " + GetComponent<UnityEngine.UI.Image> ().sprite.name);

		} catch (Exception ex) {
			Debug.Log ("TextPlayer] Error: " + ex.Message);
		}

		text = txt;
		isOn = true;

		return randomNumber;
	}

	void OnGUI(){
		//float width = (300.0f * (float)(Screen.width) / 854.0f);
		if (isOn) {
			
			Vector3 coords = Camera.main.WorldToScreenPoint(target.transform.position);
			coords.y = Screen.height - coords.y;
			//Debug.Log ("language selected : " + CrossSceneInfo.languageSelected);

//			if (CrossSceneInfo.Instance.language.languageName == CrossSceneInfo.Language.LanguagesNames.Fa) {
//				if(!guiSkin)
//					guiSkin = Resources.Load("skins/skin-sub-black", typeof(GUISkin)) as GUISkin;
//				GUI.skin = guiSkin;
//				GUI.skin.box.alignment = TextAnchor.MiddleCenter;
//				GUI.skin.box.fontSize = (int)(22.0f * (float)(Screen.width) / 854.0f);
//
//			} else if (CrossSceneInfo.Instance.language.languageName == CrossSceneInfo.Language.LanguagesNames.En) {
			if (!guiSkin)
				guiSkin = Resources.Load (CrossSceneInfo.Instance.languages [(int)CrossSceneInfo.Instance.languageSelected].languageSkinPath, typeof(GUISkin)) as GUISkin;
			GUI.skin = guiSkin;

			GUI.skin.box.padding.left = (20 * Screen.width / 854);
			GUI.skin.box.padding.right = (20 * Screen.width / 854);

			GUI.skin.box.alignment = TextAnchor.MiddleCenter;
			GUI.skin.box.fontSize = (int)(CrossSceneInfo.Instance.languages [(int)CrossSceneInfo.Instance.languageSelected].languageFontSize * (float)(Screen.width) / 854.0f);
			GUI.skin.box.font = Resources.Load<Font> (CrossSceneInfo.Instance.languages [(int)CrossSceneInfo.Instance.languageSelected].languageSubtitleFontName);
//			}

			//float height = GUI.skin.box.CalcHeight (new GUIContent (text), width);
			//GUI.Box (ClampToScreen(new Rect (coords.x - width / 2, coords.y - height - 50f, width, height)), text);
			Vector2 size = GUI.skin.box.CalcSize(new GUIContent(text));
			float yOffset = (topOffset * (float)(Screen.height) / 480.0f);
			GUI.Box (ClampToScreen(new Rect (coords.x - size.x / 2, coords.y - size.y - yOffset, size.x, size.y)), text);
		}
	}

	private Rect ClampToScreen(Rect r)
	{
		r.x = Mathf.Clamp (r.x, 0, Screen.width - r.width);
		r.y = Mathf.Clamp (r.y, 0, Screen.height - r.height);
		return r;
	}

	private void OnDisable(){
		isOn = false;
	}
}
