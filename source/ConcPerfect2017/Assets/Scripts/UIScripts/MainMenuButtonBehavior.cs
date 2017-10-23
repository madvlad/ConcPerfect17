using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonBehavior : MonoBehaviour {
    public string mainGameScene;
    public string mainMenuScene;
    public GameObject creditsUIElement;
    public GameObject escapeMenuUIElement;
    public GameObject gameStateManager;
    public GameObject mainMenuUIElement;
    public GameObject singlePlayerMenuUIElement;
    public GameObject randomSeedInputUIElement;
    public GameObject predefinedCourseMenuUIElement;
    public GameObject mouseSensitivityMenuUIElement;
    public GameObject mouseInvertYAxisMenuUIElement;
    public GameObject UnetMatchMakerToggle;
    public GameObject multiplayerMenuUIElement;
    public GameObject settingsMenuUIElement;
    public GameObject matchMakerLobbyMenuUIElement;
    public GameObject volumeSliderMenuUIElement;
    public GameObject sfxVolumeSliderMenuUIElement;
    public GameObject historyListMenuUIElement;
    public GameObject nickNameInputField;
    public GameObject loadingScreenUIElement;
    public GameObject failedMessage;
    public GameObject resetCourseButton;
    public GameObject levelPageManager;

    void Start()
    {
        if (Camera.main.GetComponent<AudioSource>() != null)
        {
            ApplicationManager.JumpsDifficultiesAllowed = new List<int> { 0, 1, 2, 3, 4 };
            //ApplicationManager.GameType = GameTypes.CasualGameType;
            Camera.main.GetComponent<AudioSource>().volume = ApplicationManager.musicVolume;
        }

        if (randomSeedInputUIElement != null)
        {
            ApplicationManager.randomSeed = 0;
        }

        if (mouseSensitivityMenuUIElement != null && mouseSensitivityMenuUIElement.GetComponent<Slider>() != null)
        {
            mouseSensitivityMenuUIElement.GetComponent<Slider>().value = ApplicationManager.mouseSensitivity;
            PlayerPrefs.SetFloat("MouseSensitivity", ApplicationManager.mouseSensitivity);
        }

		if (mouseInvertYAxisMenuUIElement != null && mouseInvertYAxisMenuUIElement.GetComponent<Toggle> () != null) {
			mouseInvertYAxisMenuUIElement.GetComponent<Toggle>().isOn = ApplicationManager.invertYAxis;
			PlayerPrefs.SetInt("MouseInvertXAxis", ApplicationManager.invertYAxis ? 1 : 0);
		}

        if (volumeSliderMenuUIElement != null)
        {
            volumeSliderMenuUIElement.GetComponent<Slider>().value = ApplicationManager.musicVolume;
        }

        if (sfxVolumeSliderMenuUIElement != null)
        {
            sfxVolumeSliderMenuUIElement.GetComponent<Slider>().value = ApplicationManager.sfxVolume;
        }

        if (resetCourseButton != null && (ApplicationManager.GameType == GameTypes.TutorialGameType || ApplicationManager.GameType == GameTypes.RaceGameType || ApplicationManager.GameType == GameTypes.ConcminationGameType))
        {
            resetCourseButton.SetActive(false);
        }
    }

    void StartNewGame9()
    {
        ApplicationManager.currentLevel = 0;
        ApplicationManager.numberOfJumps = 9;
        EnableStartHostGameButtons(false);
        loadingScreenUIElement.GetComponent<Canvas>().enabled = true;
        GameObject.FindGameObjectWithTag("NetworkIssuer").GetComponent<ConcPerfectNetworkManager>().StartNetworkManager();
    }

    void StartNewGame18()
    {
        ApplicationManager.currentLevel = 0;
        ApplicationManager.numberOfJumps = 18;
        EnableStartHostGameButtons(false);
        loadingScreenUIElement.GetComponent<Canvas>().enabled = true;
        GameObject.FindGameObjectWithTag("NetworkIssuer").GetComponent<ConcPerfectNetworkManager>().StartNetworkManager();
    }

    void StartNewGameInfinite()
    {
        ApplicationManager.numberOfJumps = 27;
        EnableStartHostGameButtons(false);
        GameObject.FindGameObjectWithTag("NetworkIssuer").GetComponent<ConcPerfectNetworkManager>().StartNetworkManager();
    }

    public void StartTutorial()
    {
        ApplicationManager.currentLevel = -1;
        ApplicationManager.numberOfJumps = 3;
        ApplicationManager.IsSingleplayer = true;
        ApplicationManager.IsLAN = true;
        ApplicationManager.GameType = GameTypes.TutorialGameType;
        EnableStartHostGameButtons(false);
        loadingScreenUIElement.GetComponent<Canvas>().enabled = true;
        GameObject.FindGameObjectWithTag("NetworkIssuer").GetComponent<ConcPerfectNetworkManager>().StartNetworkManager();
    }

    public void EnableStartHostGameButtons(bool enable) {
        foreach (GameObject button in GameObject.FindGameObjectsWithTag("StartHostGameButtons")) {
            button.GetComponent<Button>().interactable = enable;
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(mainGameScene);
        SceneManager.UnloadSceneAsync("MainMenuScene");
    }

    void ShowSingleplayerMenu()
    {
        ApplicationManager.IsSingleplayer = true;
        ApplicationManager.IsLAN = PlayerPrefs.GetInt("UseLAN") == 1 ? true : false;
        EnableStartHostGameButtons(true);
        UnetMatchMakerToggle.GetComponent<Toggle>().isOn = !ApplicationManager.IsLAN;
        predefinedCourseMenuUIElement.SetActive(false);
        singlePlayerMenuUIElement.SetActive(true);
    }

    public void ShowMultiplayerMenu()
    {
        ApplicationManager.IsSingleplayer = false;
        matchMakerLobbyMenuUIElement.SetActive(false);
        predefinedCourseMenuUIElement.SetActive(false);
        mainMenuUIElement.SetActive(false);
        multiplayerMenuUIElement.SetActive(true);
    }

    public void ShowMatchmakerServers() {
        ApplicationManager.IsLAN = false;
        multiplayerMenuUIElement.SetActive(false);
        matchMakerLobbyMenuUIElement.SetActive(false);
        matchMakerLobbyMenuUIElement.SetActive(true);
        GameObject.FindGameObjectWithTag("NetworkIssuer").GetComponent<ConcPerfectNetworkManager>().StartNetworkManager();
    }

    void ShowPredefinedCoursesMenu()
    {
        mainMenuUIElement.SetActive(false);
        singlePlayerMenuUIElement.SetActive(false);
        predefinedCourseMenuUIElement.SetActive(true);
    }

    void ShowMainMenu()
    {
        mainMenuUIElement.SetActive(true);
        singlePlayerMenuUIElement.SetActive(false);
    }

    void SetRandomSeed()
    {
        int seed = 0;
        if (int.TryParse(randomSeedInputUIElement.GetComponent<Text>().text, out seed))
        {
            if (seed != 0)
            {
                ApplicationManager.randomSeed = seed;
            }
        }
        else
        {
            // TODO :: Display some error message for the idiots trying to type letters in
        }
    }

    void LoadMainMenu()
    {
        // Reset single player menu panel to show casual
        ApplicationManager.GameType = GameTypes.CasualGameType;
        GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StopHost();
        SceneManager.UnloadSceneAsync(mainGameScene);
        SceneManager.LoadScene(mainMenuScene);
    }

    void QuitApplication()
    {
        Application.Quit();
    }

    void ShowCredits()
    {
        creditsUIElement.SetActive(true);
    }

    void HideCredits()
    {
        creditsUIElement.SetActive(false);
    }

    void HideEscapeMenu()
    {
        mouseSensitivityMenuUIElement.SetActive(false);
        var gameState = gameStateManager.GetComponent<GameStateManager>();
        gameState.SetPlayerEnabled(true);
        gameState.ShowEscapeMenu(false);
    }

    public void ShowSettings()
    {
        mouseSensitivityMenuUIElement.SetActive(true);
        escapeMenuUIElement.SetActive(false);
    }

    public void BackToMainEscapeMenu()
    {
        mouseSensitivityMenuUIElement.SetActive(false);
        escapeMenuUIElement.SetActive(true);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        ApplicationManager.mouseSensitivity = sensitivity;
        PlayerPrefs.SetFloat("MouseSensitivity", ApplicationManager.mouseSensitivity);
    }

    public void SetMouseInvertYAxis()
    {
        ApplicationManager.invertYAxis = mouseInvertYAxisMenuUIElement.GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt("InvertY", ApplicationManager.invertYAxis ? 1 : 0);
    }

    public void SetIpAddress(string ip)
    {
        ApplicationManager.NetworkAddress = ip;
    }

    public void SetNickname(string nickname) {
        if (nickname != "") {
            ApplicationManager.Nickname = nickname;
            PlayerPrefs.SetString("Nickname", nickname);
        }
    }

    public void SetServerName(string servername) {
        if (servername != "") {
            ApplicationManager.ServerName = servername;
        }
    }

    public void JoinMultiplayerGame()
    {
        failedMessage.GetComponent<Text>().enabled = false;
        loadingScreenUIElement.GetComponent<Canvas>().enabled = true;
        ApplicationManager.IsLAN = true; 
        GameObject.FindGameObjectWithTag("NetworkIssuer").GetComponent<ConcPerfectNetworkManager>().StartNetworkManager();
        Invoke("TimeOut", 20.0f);
    }

    public void TimeOut()
    {
        failedMessage.GetComponent<Text>().enabled = true;
        loadingScreenUIElement.GetComponent<Canvas>().enabled = false;
    }

    public void SetGameMode(int gameMode)
    {
        ApplicationManager.GameType = gameMode;
    }
    
    public void StartLevel(int levelNum)
    {
        ApplicationManager.IsLAN = !UnetMatchMakerToggle.GetComponent<Toggle>().isOn;
        ApplicationManager.IsSingleplayer = true;
        ApplicationManager.currentLevel = levelNum;
        ApplicationManager.numberOfJumps = 9;
        EnableStartHostGameButtons(false);
        loadingScreenUIElement.GetComponent<Canvas>().enabled = true;
        GameObject.FindGameObjectWithTag("NetworkIssuer").GetComponent<ConcPerfectNetworkManager>().StartNetworkManager();
    }

    public void ClearLevelProgress()
    {
        PlayerPrefs.SetInt("LevelsCompleted", 0);
        PlayerPrefs.DeleteKey("CourseRecords");
        PlayerPrefs.DeleteKey("RecentPlayed");
        PlayerPrefs.DeleteKey("PlayerModel");
        ApplicationManager.LevelsCompleted = 0;
        ShowMainMenu();
    }

    public void ShowSettingsMenu()
    {
        nickNameInputField.GetComponentInChildren<Text>().text = PlayerPrefs.HasKey("Nickname") ? PlayerPrefs.GetString("Nickname") : "Enter Nickname";
        mainMenuUIElement.SetActive(false);
        settingsMenuUIElement.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        mainMenuUIElement.SetActive(true);
        settingsMenuUIElement.SetActive(false);
    }

    public void GoToHistoryMenu()
    {
        settingsMenuUIElement.SetActive(false);
        historyListMenuUIElement.SetActive(true);
    }

    public void ReturnToSettingsFromHistory()
    {
        historyListMenuUIElement.SetActive(false);
        settingsMenuUIElement.SetActive(true);
    }

    public void OnUnetMatchmakerToggle() {
        ApplicationManager.IsLAN = !UnetMatchMakerToggle.GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt("UseLAN", ApplicationManager.IsLAN ? 1 : 0);
    }

    public void SetGameType(Dropdown mode)
    {
        ApplicationManager.GameType = mode.value;
        levelPageManager.GetComponent<PackSelectorScript>().ChangeGameTypeLevels(mode.value);
    }

    public void SetMusicVolume(float volume)
    {
        ApplicationManager.musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        var music = GameObject.FindGameObjectWithTag("Music");
        if (music == null)
        {
            Camera.main.GetComponent<AudioSource>().volume = ApplicationManager.musicVolume;
        }
        else
        {
            music.GetComponent<AudioSource>().volume = ApplicationManager.musicVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        ApplicationManager.sfxVolume = volume;
        PlayerPrefs.SetFloat("SfxVolume", volume);
        var sfxObjects = GameObject.FindGameObjectsWithTag("SFX");
        foreach (var sfx in sfxObjects)
        {
            sfx.GetComponent<AudioSource>().volume = volume;
        }
    }

    public void AdjustJumpComponentInclusion(bool include) {
        int difficulty = gameObject.GetComponent<LevelDifficultyValue>().DifficultyLevel;

        if (include)
        {
            ApplicationManager.JumpsDifficultiesAllowed.Add(difficulty);
        }
        else
        {
            ApplicationManager.JumpsDifficultiesAllowed.Remove(difficulty);
        }
    }

    public void RestartRun()
    {
        if (ApplicationManager.GameType != GameTypes.CasualGameType)
        {
            // Display "Can't restart during a race" or just disable button
        }
        else
        {
            gameStateManager = GameObject.FindGameObjectWithTag("GameManager");
            gameStateManager.GetComponent<GameStateManager>().SetIsCourseComplete(false);
            HideEscapeMenu();
            var player = GetLocalPlayerObject();
			player.GetComponent<LocalPlayerStats> ().UpdateStatus ("Not Started");
            player.transform.position = new Vector3(0, 2, 0);
            player.GetComponent<Concer>().SetConcCount(0);
            var gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
            gameManager.SetTimerIsRunning(false);
            gameManager.ResetTimer();
            gameManager.SetJumpNumber(0);
            gameManager.TimerHUDElement.GetComponent<Text>().text = "00:00:00";
            gameManager.JumpHUDElement.GetComponent<Text>().text = "...";
            gameManager.JumpNameHUDElement.GetComponent<Text>().text = "";
            
            var jumpSeparators = GameObject.FindGameObjectsWithTag("JumpSeparator");
            var startTrigger = GameObject.FindGameObjectWithTag("TimerTriggerOn");
            var startTriggerLabel = GameObject.FindGameObjectWithTag("TimerTriggerOn").GetComponent<SetTimerOnTrigger>().startLabel;

            startTrigger.GetComponent<MeshRenderer>().enabled = true;
            startTriggerLabel.GetComponent<MeshRenderer>().enabled = true;

            foreach(var jumpSeparator in jumpSeparators)
            {
                jumpSeparator.GetComponent<JumpTrigger>().UnsetTrigger();
            }

            var music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

            if (!music.isPlaying)
            {
                music.Play();
            }
        }
    }

    private GameObject GetLocalPlayerObject()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject playerObject = null;
        foreach (GameObject obj in playerObjects)
        {
            if (obj.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerObject = obj;
            }
        }

        return playerObject;
    }
}
