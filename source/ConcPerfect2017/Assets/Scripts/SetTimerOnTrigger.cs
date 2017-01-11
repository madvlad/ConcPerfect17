using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTimerOnTrigger : MonoBehaviour {
    public GameObject gameManager;
    public GameObject startLabel;
    public bool SwitchToOn;
    public AudioClip GameEndSound;

    private GameStateManager gameStateManager;
    private Text courseCompleteMessage;

    void Start()
    {
        gameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        courseCompleteMessage = GameObject.FindGameObjectWithTag("CourseEndText").GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameStateManager.TimerIsRunning && SwitchToOn)
        {
            gameStateManager.SetTimerIsRunning(SwitchToOn);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            startLabel.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (gameStateManager.TimerIsRunning && !SwitchToOn)
        {
            gameStateManager.SetTimerIsRunning(SwitchToOn);
            gameObject.GetComponent<AudioSource>().PlayOneShot(GameEndSound);
            GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Stop();
            courseCompleteMessage.enabled = true;
            courseCompleteMessage.text = "Course Complete!!\n\nYour time: " + gameStateManager.GetCurrentTime();
            gameStateManager.SetPlayerEnabled(false);
            Invoke("EndGame", 7.0f);
        }
    }

    private void EndGame()
    {
        courseCompleteMessage.enabled = false;
        gameStateManager.ShowEscapeMenu(true);
        gameStateManager.SetIsCourseComplete(true);
    }
}
