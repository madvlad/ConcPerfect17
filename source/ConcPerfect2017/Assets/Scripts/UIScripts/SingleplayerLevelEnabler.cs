using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleplayerLevelEnabler : MonoBehaviour {
    public GameObject Level1UIElement;
    public GameObject Level2UIElement;
    public GameObject Level3UIElement;
    public GameObject Level4UIElement;
    public GameObject Level5UIElement;

	void Start () {
		for (int i = 0; i <= ApplicationManager.LevelsCompleted; i++)
        {
            EnableLevel(i);
        }
	}

    void EnableLevel(int i)
    {
        switch(i)
        {
            case 1:
                Level2UIElement.GetComponent<Button>().interactable = true;
                break;
            case 2:
                Level3UIElement.GetComponent<Button>().interactable = true;
                break;
            case 3:
                Level4UIElement.GetComponent<Button>().interactable = true;
                break;
            case 4:
                Level4UIElement.GetComponent<Button>().interactable = true;
                break;
            default:
                Level1UIElement.GetComponent<Button>().interactable = true;
                break;
        }
    }
}
