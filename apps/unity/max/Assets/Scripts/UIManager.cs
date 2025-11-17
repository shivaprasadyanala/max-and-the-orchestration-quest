using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
	public GameObject pauseMenu;
	public GameObject dialogBox;
	public GameObject creditBox;
    public GameObject dialog_plaseWait;
	public AudioPlayer MusicBox;
	public float delayAfterSelection = 6f;
	public int prevSceneIndex = -1;

	public Button btn_newGame;
	public Button btn_continueGame;
	public Button btn_exit;

	private PlayerPhysics playerPhysics;
	private InputState playerInputState;
	private bool pauseTheTime = false;
	private float default_timeScale;
	private string clickedButtonName = string.Empty;
	private float timeElapsed = 0f;

//	void Awake(){
//		Time.timeScale = 1f;
//	}

	void Start(){
		Time.timeScale = 1f;

		try{			
			Screen.sleepTimeout = SleepTimeout.NeverSleep;

		}catch(Exception ex){
			Debug.Log ("UIManager Error : " + ex.Message);
		}

		default_timeScale = Time.timeScale;
		prevSceneIndex =  CrossSceneInfo.Instance.prevSceneIndex;

		var player = GameObject.Find ("Player");
		if (player) {
			playerPhysics = player.GetComponent<PlayerPhysics> ();
			playerInputState = player.GetComponent<InputState> ();
		}

		var fileInfo = new FileInfo (CrossSceneInfo.Instance.checkpointFilePath);
		if (!fileInfo.Exists || fileInfo.Length <= 0) {
			//UnityEngine.UI.Button btn_loadGame = GetComponent<UnityEngine.UI.Button>();
			if (btn_continueGame) {
				btn_continueGame.interactable = false;
			}
		}
			
		InitScreen ();
	}

	void Update()
	{
		if (pauseTheTime) {
			timeElapsed += Time.deltaTime;
			if (timeElapsed >= 1f && Time.timeScale > 0f) {
				Time.timeScale = 0f;
			}

		} else if (!pauseTheTime && Time.timeScale == 0f) {
			Time.timeScale = default_timeScale;
		}

		// if player Press ESC or Back
		if (Input.GetKey (KeyCode.Escape) && !playerPhysics.isDead && playerInputState.inputEnabled) {
			if (pauseMenu)
				PauseTheGame ();
		}

		if (clickedButtonName != string.Empty) {
			timeElapsed += Time.deltaTime;
			if (timeElapsed >= delayAfterSelection) {
				timeElapsed = 0f;

				switch (clickedButtonName) {
				case "newgame":
					DeleteCheckpoints ();
					CrossSceneInfo.Instance.checkppointTemp = null;
					SceneManager.LoadScene ("LoadingScene");
					clickedButtonName = string.Empty;
					//Debug.Log ("start new game");
					break;

				case "continuegame":
					SceneManager.LoadScene ("LoadingScene");
					clickedButtonName = string.Empty;
					//SceneManager.UnloadSceneAsync ("MainMenuScene");
					//Debug.Log ("continue the game");
					break;
				}
			}
		}

		if (btn_newGame && !btn_newGame.interactable) {
			var text = btn_newGame.GetComponentInChildren<Text> (true);
			text.color = Color.black;

		} else if (btn_newGame && btn_newGame.interactable) {
			var text = btn_newGame.GetComponentInChildren<Text> (true);
			text.color = Color.white;
		}

		if (btn_continueGame && !btn_continueGame.interactable) {
			var text = btn_continueGame.GetComponentInChildren<Text> (true);
			text.color = Color.black;

		} else if (btn_continueGame && btn_continueGame.interactable) {
			var text = btn_continueGame.GetComponentInChildren<Text> (true);
			text.color = Color.white;
		}

		if (btn_exit && !btn_exit.interactable) {
			var text = btn_exit.GetComponentInChildren<Text> (true);
			text.color = Color.black;

		} else if (btn_exit && btn_exit.interactable) {
			var text = btn_exit.GetComponentInChildren<Text> (true);
			text.color = Color.white;
		}

		//Debug.Log ("TimeScale = " + Time.timeScale);
	}

//	void OnApplicationPause(bool pause){
//		Debug.Log ("OnApplicationPause : " + pause);
//
//		if (pause) {
//			default_timeScale = Time.timeScale;
//			if (pauseMenu) {
//				PauseTheGame ();
//			}
//		}
//	}
//
//	void OnGUI(){
//		if (Input.GetMouseButtonDown (0) && !isClicked) {
//			Debug.Log ("mouse button down");
//			Director director = GameObject.Find ("Director").GetComponent<Director> ();
//			if (director) {
//				director.DoSkipCurrentTask ();
//			}
//
//			isClicked = true;
//		}
//		if (Input.GetMouseButtonUp (0)) {
//			isClicked = false;
//		}
//	}

    private void NewGameRoutine()
    {
        clickedButtonName = "newgame";
        CloseDialogBox(dialogBox);
        if (dialog_plaseWait)
            ShowDialogBox(dialog_plaseWait, "false");
        FadeInMainDimScreen(4f);
    }

	public void StartNewGameConfirmation(bool confirmed) {
		if (confirmed) {
            NewGameRoutine();

		} else {
			btn_continueGame.interactable = true;
			btn_exit.interactable = true;
			CloseDialogBox (dialogBox);
		}
	}

	public void StartNewGame() 
	{
		if (btn_continueGame.interactable && dialogBox) {
			//dialogBox.SetActive (true);
			ShowDialogBox(dialogBox, "false");

		} else {
            NewGameRoutine();
        }
		btn_continueGame.interactable = false;
		btn_exit.interactable = false;
	}

	public void ContinueTheGame()
	{
		btn_newGame.interactable = false;
		btn_exit.interactable = false;

        clickedButtonName = "continuegame";
		FadeInMainDimScreen (4f);

        if (dialog_plaseWait)
            ShowDialogBox(dialog_plaseWait, "false");
    }

	public void AboutCredit() 
	{
		if (creditBox)		
			ShowDialogBox (creditBox, "false");
	}

	public void ExitTheGame(){
		Application.Quit ();
	}

	public void ResumeGame (){
		//Time.timeScale = default_timeScale;
		CloseDialogBox (pauseMenu);
		//Debug.Log ("game resumed time scale: " + Time.timeScale);
	}

	public void PauseTheGame (){
		//Time.timeScale = 0;
		ShowDialogBox (pauseMenu, "true");
		//Debug.Log ("game paused time scale: " + Time.timeScale);
	}

	public void RestartTheGame(){
		SceneManager.LoadScene ("Jungle");
	}

	public void GotoMainMenu(){
		CrossSceneInfo.Instance.checkppointTemp = null;
		CrossSceneInfo.Instance.prevSceneIndex = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene ("MainMenuScene");
	}

	public void DeleteCheckpoints(){
		try {
			FileStream file = File.Create (CrossSceneInfo.Instance.checkpointFilePath);
			file.Close ();

		} catch (Exception ex) {
			Debug.Log ("checkpoint] error: " + ex.Message);
		}
	}

	public void ShowDialogBox(GameObject form, string pauseTheGame){
		try {
			if (form) {

				//Preventer #1 : dont show any dialogbox if player is dead
				if(playerPhysics && playerPhysics.isDead)
					return;

				//Debug.Log (CrossSceneInfo.DialogInfo.dialogName + " -- " + form.name + " -- " + CrossSceneInfo.DialogInfo.isDialogShown);
				GameObject go = null;
				try{
					go = form.transform.Find("Image").gameObject;
				}catch{
				}
					
				if (go && CrossSceneInfo.Instance.FindInDialogsInfo (go.GetComponent<Image>().sprite.name) != -1){
					Debug.Log("UIManager : Dialog will not be shown because it had shown before.");
					return;
				}

				if (playerInputState)
					playerInputState.inputEnabled = false;

				//Time.timeScale = 0f;
				pauseTheTime = bool.Parse (pauseTheGame);

				form.SetActive (true);

				if (go){
					CrossSceneInfo.Instance.SetDialogsInfo (go.GetComponent<Image>().sprite.name);
					Debug.Log ("UIManager Dialog " + go.GetComponent<Image>().sprite.name + " Shown.");
				}
			
//			CrossSceneInfo.DialogInfo.dialogName = form.name;
//			CrossSceneInfo.DialogInfo.isDialogShown = true;
			}
		} catch (Exception ex) {
			Debug.Log ("UIManager Error : " + ex.Message);
		}
	}
    public void ShowDialogBoxAndPause(GameObject form)
    {
        try
        {
            if (form)
            {

                //Preventer #1 : dont show any dialogbox if player is dead
                if (playerPhysics && playerPhysics.isDead)
                    return;

                ////Debug.Log (CrossSceneInfo.DialogInfo.dialogName + " -- " + form.name + " -- " + CrossSceneInfo.DialogInfo.isDialogShown);
                //GameObject go = null;
                //try
                //{
                //    go = form.transform.Find("Image").gameObject;
                //}
                //catch
                //{
                //}

                //if (go && CrossSceneInfo.Instance.FindInDialogsInfo(go.GetComponent<Image>().sprite.name) != -1)
                //{
                //    Debug.Log("UIManager : Dialog will not be shown because it had shown before.");
                //    return;
                //}

                if (playerInputState)
                    playerInputState.inputEnabled = false;

                //Time.timeScale = 0f;
                pauseTheTime = true;

                form.SetActive(true);

                //if (go)
                //{
                //    CrossSceneInfo.Instance.SetDialogsInfo(go.GetComponent<Image>().sprite.name);
                //    Debug.Log("UIManager Dialog " + go.GetComponent<Image>().sprite.name + " Shown.");
                //}

                //			CrossSceneInfo.DialogInfo.dialogName = form.name;
                //			CrossSceneInfo.DialogInfo.isDialogShown = true;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("UIManager Error : " + ex.Message);
        }
    }

    public void CloseDialogBox(GameObject form){
		if (form) {

			if (playerInputState)
				playerInputState.inputEnabled = true;

			//Time.timeScale = default_timeScale;
			pauseTheTime = false;

            timeElapsed = 0f;

			form.SetActive (false);
		}
	}

    public void ChangeScreenResolution(Dropdown dd)
    {

        string res = dd.options[dd.value].text;
        string[] values = res.Split('x');
        int nWidth = 0, nHeight = 0;
        nWidth = int.Parse(values[0]);
        nHeight = int.Parse(values[1]);
        Screen.SetResolution(nWidth, nHeight, true);
        Debug.Log("Resolution set to " + res);
    }

    private void FadeInMainDimScreen(float delay){
		if(MusicBox)
			MusicBox.Audio_Stop_Clip_Smoothly ();

        try
        {
            var dimScreen = GameObject.Find("Panel_DimScreen3") as GameObject;
            dimScreen.GetComponent<FadeObject>().delay = delay;
            dimScreen.GetComponent<FadeObject>().fadeMethod = FadeObject.FadeMethod.FadeIn;
        }
        catch
        {

        }
	}    

	private void InitScreen(){
//		TextPlayer textPlayer = GetComponent<TextPlayer> ();

		// changing text font
//		if (CrossSceneInfo.Instance.language.languageName == CrossSceneInfo.Language.LanguagesNames.Fa) {

//			Text btn_text = null;

//			var text = GameObject.Find ("txt_title");
//			if (text) {
//				btn_text = text.GetComponent<Text>();
//				btn_text.font = Resources.Load<Font> ("F_kalaam");
//				btn_text.text = textPlayer.Text_Find_Item (".0");
//			}
//
//			//text = GameObject.Find ("txt_msg");
//			if (newGameCautionTextMessage) {
//				//btn_text = text.GetComponent<Text>();
//				newGameCautionTextMessage.font = Resources.Load<Font> ("F_kalaam");
//				newGameCautionTextMessage.text = textPlayer.Text_Find_Item (".6");
//			}
//
//			if (btn_yes) {
//				btn_text = btn_yes.GetComponentInChildren<Text> ();
//				if (btn_text) {
//					btn_text.font = Resources.Load<Font> ("F_kalaam");
//					btn_text.text = textPlayer.Text_Find_Item (".7");
//				}
//			}
//			if (btn_no) {
//				btn_text = btn_no.GetComponentInChildren<Text> ();
//				if (btn_text) {
//					btn_text.font = Resources.Load<Font> ("F_kalaam");
//					btn_text.text = textPlayer.Text_Find_Item (".8");
//				}
//			}
//
//			if (btn_newGame) {
//				btn_text = btn_newGame.GetComponentInChildren<Text> ();
//				if (btn_text) {
//					btn_text.font = Resources.Load<Font> ("F_kalaam");
//					btn_text.text = textPlayer.Text_Find_Item (".1");
//				}
//			}
//
//			if (btn_continueGame) {
//				btn_text = btn_continueGame.GetComponentInChildren<Text> ();
//				if (btn_text) {
//					btn_text.font = Resources.Load<Font> ("F_kalaam");
//					btn_text.text = textPlayer.Text_Find_Item (".2");
//				}
//			}
//
//			if (btn_exit) {
//				btn_text = btn_exit.GetComponentInChildren<Text> ();
//				if (btn_text) {
//					btn_text.font = Resources.Load<Font> ("F_kalaam");
//					btn_text.text = textPlayer.Text_Find_Item (".3");
//				}
//			}

//		} else if (CrossSceneInfo.Instance.language.languageName == CrossSceneInfo.Language.LanguagesNames.En) {
			
//		}


	}
    
}
