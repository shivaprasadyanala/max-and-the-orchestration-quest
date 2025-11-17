using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CrossSceneInfo : MonoBehaviour {

	public static CrossSceneInfo Instance { get; set; }

	public Language[] languages = { 
		new Language (Language.LanguagesNames.Fa, "F_MITRA BOLD", 22.0f, "skins/skin-sub-black", "Subtitles/Fa", "F_MITRA BOLD", 22.0f),
		new Language (Language.LanguagesNames.En, "OpenSansSemibold", 18.0f, "skins/skin-sub-black2", "Subtitles/En", "OpenSansSemibold", 18.0f)
	};

    [SerializeField]
	private Language.LanguagesNames _languageSelected;
	public Language.LanguagesNames languageSelected { 
		get { return _languageSelected; }
		set{
			_languageSelected = value;
			subtitleAsset = (TextAsset)Resources.Load(languages[(int)value].languageSubtitlePath, typeof(TextAsset)); 
		}
	}

	public int prevSceneIndex = -1;
	public string checkpointFilePath;
	public Checkpoint checkppointTemp = null;
	public  bool isGameUnlocked = false;

	public TextAsset subtitleAsset;
	private List<string> dialogsInfo = null;

	private void Awake(){
		if (Instance == null) {

			checkpointFilePath = Path.Combine (Application.persistentDataPath, "checkpoints.gd");

			// Set the default language here
			languageSelected = Language.LanguagesNames.Fa;

			checkppointTemp = new Checkpoint ();
			dialogsInfo = new List<string> ();
			dialogsInfo.Add ("dialogs");

			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else {
			checkppointTemp = null;
			Destroy (gameObject);
		}
	}


	public void SetDialogsInfo(string dialogID){
		dialogsInfo.Add (dialogID);
	}
	public int FindInDialogsInfo(string dialogID){
		return dialogsInfo.LastIndexOf (dialogID);
	}

	public void SelectLanguage(int langIndex){
		languageSelected = (Language.LanguagesNames) langIndex;
	}
}
