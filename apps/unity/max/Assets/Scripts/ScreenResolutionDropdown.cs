using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolutionDropdown : MonoBehaviour
{
    // ResolutionD is name of drop menu. leave no selectable options when making the drop menu
    public Dropdown ResolutionD;

    Resolution[] resolutions;

    void Start()
    {

        resolutions = Screen.resolutions;
        if (resolutions.Length <= 0) return;

        ResolutionD.options.Clear();
        for (int i = 0; i < resolutions.Length; i++)
        //for (int i = 0; i < ResolutionD.options.Count-1; i++)
        {
            ResolutionD.options.Add(new Dropdown.OptionData(ResToString(resolutions[i])));

            //ResolutionD.value = i;

            //ResolutionD.onValueChanged.AddListener()
            //        //Screen.SetResolution(resolutions[ResolutionD.value].width, resolutions[ResolutionD.value].height, true);
            //        //Debug.Log(ResolutionD.options[ResolutionD.value].text);
            //        SetResolution(ResolutionD.options[ResolutionD.value].text);
            //    });

            //}

            //foreach(Dropdown.OptionData option in ResolutionD.options)
            //{

        }
    }
    // this goes outside of "void start". it puts resolution options into a string to be displayed in drop menu
    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }

}
