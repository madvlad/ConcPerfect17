using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairUIScript : MonoBehaviour {

    public GameObject CrossHair;

    void Start () {
        if (PlayerPrefs.HasKey("CrosshairOn"))
        {
            var IsOn = PlayerPrefs.GetInt("CrosshairOn");
            if (IsOn == 1)
            {
                GetComponent<Toggle>().isOn = true;
                return;
            }
        }

        GetComponent<Toggle>().isOn = false;
    }

    public void ToggleCrosshairVisibility(bool enabled)
    {
        if (CrossHair != null)
        {
            CrossHair.GetComponent<Image>().enabled = enabled;
        }

        if (enabled)
        {
            PlayerPrefs.SetInt("CrosshairOn", 1);
        }
        else
        {
            PlayerPrefs.SetInt("CrosshairOn", 0);
        }
    }
}
