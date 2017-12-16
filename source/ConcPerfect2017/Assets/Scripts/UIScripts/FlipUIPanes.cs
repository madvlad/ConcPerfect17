using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipUIPanes : MonoBehaviour {
    public GameObject UIElementToHide;
    public GameObject UIElementToShow;
	
    public void FlipUI()
    {
        UIElementToHide.SetActive(false);
        UIElementToShow.SetActive(true);
    }
}
