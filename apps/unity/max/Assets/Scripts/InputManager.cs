using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public enum Buttons{
	Right,
	Left,
	Up,
	Down,
	A,
	B,
	I,
	J
}

public enum Condition{
	GreaterThan,
	LessThan
}

[System.Serializable]
public class InputAxisState{
	public string axisName;
	public float offValue;
	public Buttons button;
	public Condition condition;

	public bool value{

		get{
			//var val = Input.GetAxis(axisName);
			var val = CrossPlatformInputManager.GetAxis(axisName);
			//Debug.Log (val);
			switch(condition){
			case Condition.GreaterThan:
				return val > offValue;
			case Condition.LessThan:
				return val < offValue;
			}

			return false;
		}

	}
}

public class InputManager : MonoBehaviour {

	public InputAxisState[] inputs;
	public InputState inputState;


	// Use this for initialization
	void Start () {
		if (!inputState) {
			inputState = GameObject.FindObjectOfType<InputState> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var input in inputs) {
			inputState.SetButtonValue (input.button, input.value);
		}
	}


}
