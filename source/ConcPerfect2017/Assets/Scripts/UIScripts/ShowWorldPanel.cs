using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWorldPanel : MonoBehaviour {

    public GameObject WorldPanel;
    public GameObject OtherPanel;
    public GameObject CustomCoursePanel;

    public void Show()
    {
        WorldPanel.SetActive(true);
        OtherPanel.SetActive(false);
        CustomCoursePanel.SetActive(false);
    }

}
