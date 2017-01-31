using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour {
    static public int numberOfJumps = 9;
    static public int randomSeed = 0;
    static public int currentLevel = 0;
    static public float mouseSensitivity = 0.0f;
    static public bool invertYAxis = false;
    static public bool IsSingleplayer = true;
    static public string NetworkAddress = "localhost";
    static public int GameType = GameTypes.CasualGameType;
    static public int LevelsCompleted = 0;

    void Start()
    {
        CheckCompletedLevels();
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        invertYAxis = PlayerPrefs.GetInt("InvertY") == 1 ? true : false;
        if (mouseSensitivity == 0)
        {
            mouseSensitivity = 4;
        }
    }

    void CheckCompletedLevels()
    {
        LevelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
    }
}
