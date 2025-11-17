using UnityEngine;
using UnityEngine.UI;

public class FadeObject : MonoBehaviour {

	public enum FadeMethod
	{
		FadeIn=0,
		FadeOut,
		Blink,
		Fixed
	}

	public enum ObjectType
	{
		Sprite=0,
		Text,
		Light,
		Image
	}
		
	public ObjectType objectType;
	public FadeMethod fadeMethod = FadeMethod.FadeOut;
	public float delay = 3;

	private FadeMethod prevFadeMethod;
	private float timeElapsed = 0.0f;
	private Color startColor;
	private Color endColor;
	private Renderer comp;
	private Text text;
	private Image image;
	//private Light light;
	// Use this for initialization
	void Start () {
        timeElapsed = 0.0f;
        comp = GetComponent<SpriteRenderer> ();
		text = GetComponent<Text> ();
		image = GetComponent<Image> ();

		if (objectType != ObjectType.Light) {
			startColor = objectType == ObjectType.Sprite ? comp.material.color : objectType == ObjectType.Image ? image.color : text.color;
			endColor = new Color (startColor.r, startColor.g, startColor.b, 0f);
		}

		if (fadeMethod == FadeMethod.FadeIn) {
			if (objectType == ObjectType.Sprite)
				comp.material.color = new Color (startColor.r, startColor.g, startColor.b, 0f);
			
			else if(objectType == ObjectType.Image)
				image.color = new Color (startColor.r, startColor.g, startColor.b, 0f);
			
			else
				text.color = new Color (startColor.r, startColor.g, startColor.b, 0f);
			
			Color tempColor = endColor;
			endColor = startColor;
			startColor = tempColor;
		}

		prevFadeMethod = fadeMethod;
	}
		
//	void OnMouseUp(){
//		Debug.Log ("here up11");
//		Destroy (gameObject);
//	}

	// Update is called once per frame
	void Update () {		

		if (fadeMethod != prevFadeMethod && fadeMethod != FadeMethod.Blink) {

			if (prevFadeMethod != FadeMethod.Fixed) {
				Color tempColor = endColor;
				endColor = startColor;
				startColor = tempColor;
			}

			prevFadeMethod = fadeMethod;
			timeElapsed = 0f;
		}

		if (fadeMethod != FadeMethod.Fixed && fadeMethod != FadeMethod.Blink) {
			timeElapsed += Time.deltaTime / delay;

			if (objectType == ObjectType.Sprite)
				comp.material.color = Color.Lerp (startColor, endColor, timeElapsed);
			
			else if (objectType == ObjectType.Image)
				image.color = Color.Lerp (startColor, endColor, timeElapsed);
			
			else
				text.color = Color.Lerp (startColor, endColor, timeElapsed);
		}

		if (fadeMethod == FadeMethod.Blink) {
			if (objectType == ObjectType.Text)
				text.color = new Color (text.color.r, text.color.g, text.color.b, Mathf.PingPong (Time.time, 1f));
			
			else if (objectType == ObjectType.Light)
				GetComponent<Light>().intensity = Mathf.PingPong (Time.time * UnityEngine.Random.Range(1f, 2f), 3f);

			else if(objectType == ObjectType.Image)
				image.color = new Color (image.color.r, image.color.g, image.color.b, Mathf.PingPong (Time.time, delay));
			
			else
				comp.material.color = new Color(comp.material.color.r, comp.material.color.g, comp.material.color.b, Mathf.PingPong(Time.time, 1f));
		}
	}

    public void ResetValues()
    {
        Start();
    }
}
