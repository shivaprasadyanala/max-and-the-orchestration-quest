using UnityEngine;
using System;

public class AudioPlayer : MonoBehaviour {
    public enum OnEnableActions
    {
        Nothing = 0,
        Play,
        PlayIndependently,
    }

    public AudioClip[] audio_clips;

    [SerializeField]
    private int _clipsIndexBegin = 0;
    public int clipsIndexBegin {
        get { return _clipsIndexBegin; }
        set {
            _clipsIndexBegin = value;
            Audio_Select_File();
        }
    }
    [SerializeField]
    private int _clipsIndexEnd = 0;
    public int clipsIndexEnd {
        get { return _clipsIndexEnd; }
        set {
            _clipsIndexEnd = value;
            Audio_Select_File();
        }
    }
    public bool isPlaying;
    public OnEnableActions onEnableAction = OnEnableActions.Nothing;
    public int priority = 100;

    public bool isLoop;
    [Range(0.0f, 1.0f)]
    public float volume = 1f;
    [Range(0.0f, 1.0f)]
    public float spatialBlend = 1f;
    public float maxDistance = 60f;
    public float smoothStopRate = .001f;

    private AudioSource audioSource;
    private int prev_clipsIndexBegin;
    private int prev_clipsIndexEnd;
    private float randomPlayNumber = 0f;
    private float randomPlayRangeMax = 0f;
    private float randomPlayRangeMin = .1f;
    private bool stopSmoothly = false;
    private float default_volume = 0f;
    private float timeElapsed = 0f;

    // Use this for initialization
    void Start() {
        if(!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = isLoop;
        audioSource.volume = volume;
        audioSource.spatialBlend = spatialBlend;
        audioSource.priority = priority;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Custom;

        GameObject audioCustomCurve1 = GameObject.Find("AudioCustomCurve1");
        if (audioCustomCurve1) {
            var curve = audioCustomCurve1.GetComponent<AudioSource>().GetCustomCurve(AudioSourceCurveType.CustomRolloff);
            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);
        } else {
            Debug.Log("Audio Error : AudioCustomCurve1 not found.");
        }

        Audio_Select_File();
        prev_clipsIndexBegin = _clipsIndexBegin;
        prev_clipsIndexEnd = _clipsIndexEnd;

        default_volume = volume;
    }

    void OnEnable()
    {
        if (onEnableAction == OnEnableActions.PlayIndependently)
        {
            Start();
            Audio_Play_Independently();

        }
        else if (onEnableAction == OnEnableActions.Play)
        {
            Start();
            Audio_Play_Clip();

        }        

    }
	// Update is called once per frame
	void Update () {
		if (_clipsIndexBegin > _clipsIndexEnd)
			_clipsIndexEnd = _clipsIndexBegin;

		if (_clipsIndexBegin != prev_clipsIndexBegin) {
			Audio_Select_File ();
			prev_clipsIndexBegin = _clipsIndexBegin;
		}
		if (_clipsIndexEnd != prev_clipsIndexEnd) {
			Audio_Select_File ();
			prev_clipsIndexEnd = _clipsIndexEnd;
		}

		if(volume != audioSource.volume)
			audioSource.volume = volume;

		if (isLoop != audioSource.loop)
			audioSource.loop = isLoop;
		
		if(spatialBlend != audioSource.spatialBlend)
			audioSource.spatialBlend = spatialBlend;

		if (priority != audioSource.priority)
			audioSource.priority = priority;

		if (maxDistance != audioSource.maxDistance)
			audioSource.maxDistance = maxDistance;

		isPlaying = audioSource.isPlaying;

//		if (!audioSource.isPlaying)
//			play = false;
//		
//		if (play && !audioSource.isPlaying) {
//			audioSource.Play ();
//
//		} else if (!play && audioSource.isPlaying) {
//			audioSource.Stop ();
//		}

		if (randomPlayRangeMax > 0f) {
			timeElapsed += Time.deltaTime;

			if (timeElapsed >= randomPlayNumber) {
				Audio_Play_Independently ();

				randomPlayNumber = Get_Random_Number (randomPlayRangeMax);
				timeElapsed = 0f;
			}
		}

		if (stopSmoothly) {
			volume -= smoothStopRate;
			if (volume <= 0f) {
				volume = default_volume;
				stopSmoothly = false;
				Audio_Stop_Clip ();
			}
		}
	}

	public void Audio_Load_And_Play_File(string fileName){
		if (audioSource.clip && "Music/" + audioSource.clip.name == fileName) {
			Debug.Log ("clip name: " + "Music/" + audioSource.clip.name);
			return;
		}			
		audioSource.clip = Resources.Load<AudioClip> (fileName);
		audioSource.Play ();
		//Debug.Log ("clip name: " + audioSource.clip.name);
	}

	public void Audio_Select_File(){
		
//		AudioClip[] audio_clips = null;

//		if (clipsIndex == 1)
//			audio_clips = audio_clips1;
//
//		else if (clipsIndex == 2)
//			audio_clips = audio_clips2;
//
//		else if (clipsIndex == 3)
//			audio_clips = audio_clips3;
//
//		else if (clipsIndex == 4)
//			audio_clips = audio_clips4;

//		if (audio_clips != null && audio_clips.Length > 0) {
//			int randomNum = UnityEngine.Random.Range (0, audio_clips.Length);
//			//Debug.Log ("random number " + randomNum + " ------ " + audio_clips.Length);
//			//AudioSource.PlayClipAtPoint (audio_step [randomNum], Camera.main.transform.position, 1f);
//			audioSource.clip = audio_clips[randomNum];
//		}

		if (audio_clips != null && audio_clips.Length > 0) {
			int randomNum = UnityEngine.Random.Range (_clipsIndexBegin, _clipsIndexEnd - 1);
			//Debug.Log ("random number " + randomNum + " ------ " + audio_clips.Length + " >> " + gameObject.name);
			if(randomNum >= 0 && randomNum < audio_clips.Length)
				audioSource.clip = audio_clips[randomNum];
		}
	}

	public void SetClipIndexBegin(int newClipIndexBegin){
		_clipsIndexBegin = newClipIndexBegin;
	}
	public void SetClipIndexEnd(int newClipIndexEnd){
		_clipsIndexEnd = newClipIndexEnd;
	}
	public void SetVolume(float vol){
		volume = vol;
	}
	public void SetMinimumRandomRange(float min){
		randomPlayRangeMin = min;
	}

	public void Audio_Play_Clip(){
		if(!audioSource.isPlaying)
			audioSource.Play ();
//		play = true;
	}

	public void Audio_Stop_Clip(){
		if(audioSource.isPlaying)
			audioSource.Stop ();

		randomPlayRangeMax = 0f;
	}

	public void Audio_Stop_Clip_Smoothly(){
		if(audioSource.isPlaying)
			stopSmoothly = true;
	}

	public void Audio_Pause_Clip(){
		if (audioSource.isPlaying)
			audioSource.Pause ();

		//randomPlayRangeMax = 0f;
	}

	public void Audio_Play_Independently_At_Indices(int indexBegin, int indexEnd, float volume = 1f){
		//AudioSource.PlayClipAtPoint (audio_clips[UnityEngine.Random.Range (indexBegin, indexEnd+1)], transform.position, volume);
		audioSource.PlayOneShot(audio_clips[UnityEngine.Random.Range (indexBegin, indexEnd+1)], volume);
	}
	public void Audio_Play_Independently_At_Index(int index){
		//AudioSource.PlayClipAtPoint (audio_clips[index], transform.position, volume);
		audioSource.PlayOneShot(audio_clips[index], volume);
	}
	public void Audio_Play_Independently_At_Volume(float vol){
		//AudioSource.PlayClipAtPoint (audio_clips[index], transform.position, volume);
		//audioSource.PlayOneShot(audio_clips[_clipsIndexBegin], vol);
		audioSource.PlayOneShot(audio_clips[UnityEngine.Random.Range(_clipsIndexBegin, _clipsIndexEnd+1)], vol);
	}
	public void Audio_Play_Independently(){
		//AudioSource.PlayClipAtPoint (audio_clips[index], transform.position, volume);
		audioSource.PlayOneShot (audio_clips[UnityEngine.Random.Range (_clipsIndexBegin, _clipsIndexEnd+1)], volume);
	}
//	public void Audio_Play_Independently_At_Position(GameObject go){
//		Transform trans = null;
//		if (go)
//			trans = go.transform;
//		else
//			trans = Camera.main.transform;
//		//AudioSource.PlayClipAtPoint (audio_clips [UnityEngine.Random.Range (_clipsIndexBegin, _clipsIndexEnd + 1)], trans.position, volume);
//		audioSource.PlayOneShot(audio_clips [UnityEngine.Random.Range (_clipsIndexBegin, _clipsIndexEnd + 1)], volume);
//		//Debug.Log ("random number : " + UnityEngine.Random.Range (_clipsIndexBegin, _clipsIndexEnd + 1));
//	}
	public void Audio_Play_Independently_Randomly(string randomRangeMax){
		randomPlayRangeMax = float.Parse(randomRangeMax);
		randomPlayNumber = Get_Random_Number(randomPlayRangeMax);
	}
	private float Get_Random_Number(float max){
		return UnityEngine.Random.Range(randomPlayRangeMin, max);
	}
}
