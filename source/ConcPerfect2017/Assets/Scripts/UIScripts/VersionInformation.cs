using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionInformation : MonoBehaviour {

    public Text textUI;

	void Start () {
        textUI.text = ApplicationManager.APPLICATION_VERSION;	
	}
}
