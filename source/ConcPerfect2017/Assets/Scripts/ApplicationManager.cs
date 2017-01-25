using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour {
    static public int numberOfJumps = 9;
    static public int randomSeed = 0;
    static public int currentLevel = 0;
    static public float mouseSensitivity = 4.0f;
    static public bool IsSingleplayer = true;
    static public string NetworkAddress = "localhost";

    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        if (mouseSensitivity == 0)
        {
            mouseSensitivity = 4;
        }
    }
}
