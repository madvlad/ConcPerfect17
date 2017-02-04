﻿using System;
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
    public GameObject multiplayerMenuUIElement;
    public GameObject nicknameMenuUIElement;
    public GameObject settingsMenuUIElement;

    void Start()
    {
        if (mouseSensitivityMenuUIElement != null && mouseSensitivityMenuUIElement.GetComponent<Slider>() != null)
        {
            mouseSensitivityMenuUIElement.GetComponent<Slider>().value = ApplicationManager.mouseSensitivity;
            PlayerPrefs.SetFloat("MouseSensitivity", ApplicationManager.mouseSensitivity);
        }

		if (mouseInvertYAxisMenuUIElement != null && mouseInvertYAxisMenuUIElement.GetComponent<Toggle> () != null) {
			mouseInvertYAxisMenuUIElement.GetComponent<Toggle>().isOn = ApplicationManager.invertYAxis;
			PlayerPrefs.SetInt("MouseInvertXAxis", ApplicationManager.invertYAxis ? 1 : 0);
		}
    }

    void StartNewGame9()
    {
        ApplicationManager.currentLevel = 0;
        ApplicationManager.numberOfJumps = 9;
        ApplicationManager.GameType = 0;
        LoadGameScene();
    }

    void StartNewGame18()
    {
        ApplicationManager.currentLevel = 0;
        ApplicationManager.numberOfJumps = 18;
        ApplicationManager.GameType = 0;
        LoadGameScene();
    }

    void StartNewGameInfinite()
    {
        ApplicationManager.numberOfJumps = 27;
        LoadGameScene();
    }

    public void StartTutorial()
    {
        ApplicationManager.currentLevel = -1;
        ApplicationManager.numberOfJumps = 3;
        ApplicationManager.IsSingleplayer = true;
        ApplicationManager.GameType = 1;
        LoadGameScene();
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene(mainGameScene);
        SceneManager.UnloadSceneAsync("MainMenuScene");
    }

    void ShowSingleplayerMenu()
    {
        ApplicationManager.IsSingleplayer = true;
        predefinedCourseMenuUIElement.SetActive(false);
        mainMenuUIElement.SetActive(false);
        nicknameMenuUIElement.SetActive(true);
        singlePlayerMenuUIElement.SetActive(false);
    }

    public void ShowMultiplayerMenu()
    {
        ApplicationManager.IsSingleplayer = false;
        predefinedCourseMenuUIElement.SetActive(false);
        mainMenuUIElement.SetActive(false);
        nicknameMenuUIElement.SetActive(true);
        multiplayerMenuUIElement.SetActive(false);
    }

    void ShowPredefinedCoursesMenu()
    {
        singlePlayerMenuUIElement.SetActive(false);
        multiplayerMenuUIElement.SetActive(false);
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
            ApplicationManager.randomSeed = seed;
        } else
        {
            // TODO :: Display some error message for the idiots trying to type letters in
        }
    }

    void LoadMainMenu()
    {
        GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StopHost();
        SceneManager.LoadScene(mainMenuScene);
        SceneManager.UnloadSceneAsync(mainGameScene);
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
        ApplicationManager.Nickname = nickname;
    }

    public void OnEnterNickname() {
        if (ApplicationManager.IsSingleplayer) {
            nicknameMenuUIElement.SetActive(false);
            singlePlayerMenuUIElement.SetActive(true);
        } else {
            nicknameMenuUIElement.SetActive(false);
            multiplayerMenuUIElement.SetActive(true);
        }
    }

    public void JoinMultiplayerGame()
    {
        LoadGameScene();
    }

    public void SetGameMode(int gameMode)
    {
        ApplicationManager.GameType = gameMode;
    }
    
    public void StartLevel(int levelNum)
    {
        ApplicationManager.currentLevel = levelNum;
        ApplicationManager.numberOfJumps = 9;
        ApplicationManager.GameType = 0;
        LoadGameScene();
    }

    public void ClearLevelProgress()
    {
        PlayerPrefs.SetInt("LevelsCompleted", 0);
        ApplicationManager.LevelsCompleted = 0;
        ShowMainMenu();
    }

    public void ShowSettingsMenu()
    {
        mainMenuUIElement.SetActive(false);
        settingsMenuUIElement.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        mainMenuUIElement.SetActive(true);
        settingsMenuUIElement.SetActive(false);
    }
}
