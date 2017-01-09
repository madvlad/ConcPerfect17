using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonBehavior : MonoBehaviour {
    public string sceneToLoad;

    void StartNewGame()
    {
        SceneManager.LoadScene(sceneToLoad);
        SceneManager.UnloadSceneAsync("MainMenuScene");
    }

    void QuitApplication()
    {
        Application.Quit();
    }
}
