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
	public GameObject Level6UIElement;

	void Start () {
        for (int i = 0; i <= ApplicationManager.LevelsCompleted; i++)
        {
            EnableLevel(i);
        }
    }

    void OnEnable()
    {
        Level2UIElement.GetComponent<Button>().interactable = false;
        Level3UIElement.GetComponent<Button>().interactable = false;
        Level4UIElement.GetComponent<Button>().interactable = false;
        Level5UIElement.GetComponent<Button>().interactable = false;
		Level6UIElement.GetComponent<Button>().interactable = false;
		Start();
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
                Level5UIElement.GetComponent<Button>().interactable = true;
                break;
			case 5:
				Level6UIElement.GetComponent<Button>().interactable = true;
				break;
			default:
                Level1UIElement.GetComponent<Button>().interactable = true;
                break;
        }
    }
}
