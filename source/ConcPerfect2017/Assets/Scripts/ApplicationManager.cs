using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ApplicationManager : MonoBehaviour {
    public const string APPLICATION_VERSION = "1.1.0";

    static public float musicVolume = 0.5f;
    static public float sfxVolume = 0.5f;
    static public int numberOfJumps = 9;
    static public int randomSeed = 0;
    static public int currentLevel = 0;
    static public uint MaxNumberOfPlayers = 4;
    static public float mouseSensitivity = 0.0f;
    static public bool invertYAxis = false;
    static public bool IsSingleplayer = true;
    static public bool AdvertiseServer = true;
    static public bool IsLAN = false;
    static public string ServerPassword = "";
    static public string NetworkAddress = "localhost";
    static public string Nickname = "0xD15EA5E";
    static public string ServerName = "0xDEADBEAF";
    static public int GameType = GameTypes.CasualGameType;
    static public int LevelsCompleted = 0;
    static public List<int> JumpsDifficultiesAllowed = new List<int> { 0, 1, 2, 3, 4 };

    private string[] defaultNicknames = { "BAADF00D", "D15EA5E", "1CEB00DA", "DEADBEAF" };
    void Start()
    {
        CheckCompletedLevels();
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume");

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = 0.4f;
        }
        if (!PlayerPrefs.HasKey("SfxVolume"))
        {
            sfxVolume = 0.4f;
        }

        invertYAxis = PlayerPrefs.GetInt("InvertY") == 1 ? true : false;
        if (mouseSensitivity == 0)
        {
            mouseSensitivity = 4;
        }

        if (PlayerPrefs.HasKey("Nickname")) {
            ApplicationManager.Nickname = PlayerPrefs.GetString("Nickname");
        } else {

            ApplicationManager.Nickname = defaultNicknames[Random.Range(0, 4)];
        }
        ApplicationManager.ServerName = ApplicationManager.Nickname;

    }

    void CheckCompletedLevels()
    {
        LevelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
    }

    public static int GetDifficultyLevel()
    {
        var sum = 0;

        foreach (var difficulty in ApplicationManager.JumpsDifficultiesAllowed)
        {
            sum += GetDifficultyIndex(difficulty);
        }

        return sum;
    }

    static int GetDifficultyIndex(int jumpDifficulty)
    {
        switch (jumpDifficulty)
        {
            case 0:
                return 1;
            case 1:
                return 2;
            case 2:
                return 4;
            case 3:
                return 8;
            case 4:
                return 16;
            case 5:
                return 32;
        }

        return 0;
    }
}
