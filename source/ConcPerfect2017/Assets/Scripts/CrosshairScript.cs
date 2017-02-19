using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairScript : MonoBehaviour {
	void Start () {
        if (PlayerPrefs.HasKey("CrosshairOn"))
        {
            var IsOn = PlayerPrefs.GetInt("CrosshairOn");
            if (IsOn == 1)
            {
                GetComponent<Image>().enabled = true;
                return;
            }
        }

        GetComponent<Image>().enabled = false;
    }
}
