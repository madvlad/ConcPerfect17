using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTimerOnTrigger : MonoBehaviour {
    public GameObject gameManager;
    public GameObject startLabel;
    public bool SwitchToOn;
    public AudioClip GameEndSound;
    public ParticleSystem confetti1;
    public ParticleSystem confetti2;
    public ParticleSystem confetti3;

    private GameStateManager gameStateManager;
    private Text courseCompleteMessage;

    void Start()
    {
        gameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        courseCompleteMessage = GameObject.FindGameObjectWithTag("CourseEndText").GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
                ShootConfetti(other.gameObject);
                gameStateManager.SetIsCourseComplete(true);
                gameStateManager.SetPlayerEnabled(false);
                Invoke("EndGame", 7.0f);
            }
        }
    }

    private void ShootConfetti(GameObject player)
    {
        confetti1.transform.position = player.transform.position + player.transform.forward - player.transform.up;
        confetti2.transform.position = player.transform.position + player.transform.forward - player.transform.up;
        confetti3.transform.position = player.transform.position + player.transform.forward - player.transform.up;
        confetti1.Emit(100);
        confetti2.Emit(100);
        confetti3.Emit(100);
    }

    private void EndGame()
    {
        courseCompleteMessage.enabled = false;
        gameStateManager.ShowEscapeMenu(true);
    }
}
