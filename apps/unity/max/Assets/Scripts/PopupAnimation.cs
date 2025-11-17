using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAnimation : MonoBehaviour
{
    private float xlength;
    private float rate;

    public float sizeOfGUI = 1;
    //public AudioClip sound_popup;

    //private AudioPlayer ap;
    //private Color alphaColor;
    //private float timeToFade = 1.0f;

    void Start()
    {
        //ap = gameObject.AddComponent<AudioPlayer>();
        //ap.audio_clips = new AudioClip[] { sound_popup };
        //ap.maxDistance = 60;
        //ap.priority = 1;
        //xlength = transform.localScale.x;

        //alphaColor = GetComponent<Renderer>().material.color;
        //alphaColor.a = 0;
    }

    void OnEnable()
    {
        ResetValues();
        
        StartCoroutine("BubbleAnimation");
        try
        {
            //GetComponentInChildren<FadeObject>().fadeMethod = FadeObject.FadeMethod.FadeIn;
            GetComponentInChildren<FadeObject>().enabled = true;
            
        }
        catch(Exception ex)
        {
            Debug.Log("! PopupAnimation Error - FadeObject: " + ex.Message);
        }
    }
    void OnDisable()
    {
        ResetValues();
        StopCoroutine("BubbleAnimation");
        try
        {
            //GetComponentInChildren<FadeObject>().fadeMethod = FadeObject.FadeMethod.FadeOut;            
            GetComponentInChildren<FadeObject>().ResetValues();
            GetComponentInChildren<FadeObject>().enabled = false;
        }
        catch (Exception ex)
        {
            Debug.Log("! PopupAnimation Error - FadeObject: " + ex.Message);
        }
    }

    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.A))
        //{
            //ResetValues();
            //StartCoroutine("BubbleAnimation");
            //         GetComponentInChildren<FadeObject>().enabled = true;
        //}

        transform.localScale = new Vector3(xlength, xlength, xlength);
    }

    IEnumerator BubbleAnimation()
    {
        int step = new int();
        float lengthLocal = new float();
        lengthLocal = sizeOfGUI;
        while (step < 4)
        {
            if (step == 0)
            {
                if (xlength < lengthLocal)
                {

                    //Debug.Log("herererer...1111111111111111111111");
                    // increase the size with speed 20, +1 to increase 
                    AnimController(0.1f, 1);

                    //GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, alphaColor, timeToFade * Time.deltaTime);
                }
                else
                {
                    //Debug.Log("herererer...1111111555555555");
                    step = 2;
                    rate = 0.05f;
                    
                }
            }
            else if (step == 2)
            {
                if (xlength > lengthLocal - .2f)
                {
                    //Debug.Log("herererer...22222222222");
                    // decrease the size with speed 10, -1 to decrease
                    AnimController(0.02f, -1);
                }
                else
                {
                    step = 3;
                    rate = .05f;
                    //Debug.Log("herererer...222225555555");
                }
            }
            else if (step == 3)
            {
                if (xlength < lengthLocal)
                {
                    // increase the size with speed 10, +1 to increase 
                    AnimController(0.0005f, 1);
                    if (xlength >= lengthLocal)
                    {
                        xlength = lengthLocal;
                        step = 4;
                        //Debug.Log("herererer...3333");
                    }

                }
                else
                {
                    step = 4;
                    //rate  = 0.2f;
                }
            }
            yield return null;
        }
        StopCoroutine("BubbleAnimation");
    }

    void AnimController(float ammount, int gear)
    {
        rate += Time.deltaTime * ammount * gear;
        xlength += rate * gear;
    }

    void ResetValues()
    {
        xlength = transform.localScale.x;
        //xlength = sizeOfGUI / 2f;
        rate = 0;
    }
}
