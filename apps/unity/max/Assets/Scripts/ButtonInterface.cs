using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonInterface : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public Vector3 pressed_labelPositionOffset;
    public AudioClip down_sound;
    public AudioClip up_sound;
    public AudioClip hover_sound;

    private Text label;
    private Vector3 newPositionOffset;
    private Vector3 oldPosition;
    private bool isClickDown = false;
    private AudioPlayer ap;

    void Start()
    {
        label = GetComponentInChildren<Text>();
        if (label)
            //oldPosition = label.transform.position;
            oldPosition = label.GetComponent<RectTransform>().localPosition;

        ap = gameObject.AddComponent<AudioPlayer>();

        //if (down_sound && !hover_sound)
        //{
        //    ap.audio_clips = new AudioClip[] { down_sound };
        //}
        //else if (down_sound && hover_sound)
        //{
            ap.audio_clips = new AudioClip[] { down_sound, up_sound, hover_sound };
        //}
        
        ap.maxDistance = 400;
    }

    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClickDown = true;

        PerformDownActions();

        //Debug.Log("OnPointDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClickDown = false;

        PerformUpActions();
    }

    public void OnPointerExit(PointerEventData eventData)
    {        
        if (isClickDown)
            PerformUpActions();
        //Debug.Log("OnPointExit");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PerformHoverActions();

        //Debug.Log("OnPointEnter");
        if (isClickDown && label)
        {
            PerformDownActions();
        }
    }

    private void PopulateOffset()
    {
        float buttonHeight = GetComponent<RectTransform>().rect.height;
        //CameraPixelPerfect ppc = Camera.main.GetComponent<CameraPixelPerfect>();
        //pressed_labelPositionOffset = new Vector3(pressed_labelPositionOffset.x, (-.05f/ppc.nativeResolution.y)*Screen.height, pressed_labelPositionOffset.z);

        //pressed_labelPositionOffset = new Vector3((pressed_labelPositionOffset.x * Screen.width) / 800f, (pressed_labelPositionOffset.y * (float)Screen.height) / 480f, pressed_labelPositionOffset.z);
        //pressed_labelPositionOffset = new Vector3((pressed_labelPositionOffset.x * transform.localScale.x) / 1f, (pressed_labelPositionOffset.y * 1f) / transform.localScale.y, pressed_labelPositionOffset.z);
        //Debug.Log("Screen Height : " + Screen.height);
        //newPositionOffset = new Vector3(pressed_labelPositionOffset.x, (pressed_labelPositionOffset.y * (Screen.height+buttonHeight)) / (800f+60f), pressed_labelPositionOffset.z);        
        newPositionOffset = new Vector3(pressed_labelPositionOffset.x, (pressed_labelPositionOffset.y * buttonHeight) / 65f, pressed_labelPositionOffset.z);      //65f is sample button height  
        //Debug.Log("OFFSET : " + newPositionOffset.y);
        //Debug.Log("localScale : " + transform.localScale.y);
        //(300.0f * (float)(Screen.width) / 854.0f);
        //rectX = (_rect.x / ppc.nativeResolution.x) * Screen.width;
        //float rectY = (_rect.y / ppc.nativeResolution.y) * Screen.height;
    }

    private void PerformDownActions()
    {
        if (!GetComponent<Button>().interactable)
            return;

        PopulateOffset();

        if (label)
            //label.transform.position += pressed_labelPositionOffset;
            label.GetComponent<RectTransform>().anchoredPosition += (Vector2)newPositionOffset;
        //Debug.Log("NEW POSITION : " + label.GetComponent<RectTransform>().anchoredPosition);

        if (down_sound)
            ap.Audio_Play_Independently_At_Index(0);
    }
    private void PerformUpActions()
    {
        if (label)
            label.GetComponent<RectTransform>().localPosition = oldPosition;
        
        if (up_sound)
            ap.Audio_Play_Independently_At_Index(1);
    }
    private void PerformHoverActions()
    {
        if (hover_sound)
            ap.Audio_Play_Independently_At_Index(2);
    }
}
