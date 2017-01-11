using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    void StartNewGame9()
    {
        ApplicationManager.numberOfJumps = 9;
        LoadGameScene();
    }

    void StartNewGame18()
    {
        ApplicationManager.numberOfJumps = 18;
        LoadGameScene();
    }

    void StartNewGameInfinite()
    {
        ApplicationManager.numberOfJumps = 27;
        LoadGameScene();
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene(mainGameScene);
        SceneManager.UnloadSceneAsync("MainMenuScene");
    }

    void ShowSingleplayerMenu()
    {
        mainMenuUIElement.SetActive(false);
        singlePlayerMenuUIElement.SetActive(true);
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
        var gameState = gameStateManager.GetComponent<GameStateManager>();
        gameState.SetPlayerEnabled(true);
        gameState.ShowEscapeMenu(false);
    }
}
