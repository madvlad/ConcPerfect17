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
    private CourseHistoryManager courseHistoryManager;
    private Text courseCompleteMessage;

    void Start()
    {
        gameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        courseHistoryManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CourseHistoryManager>();
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

                if (gameStateManager.GetLocalPlayerObject () != null)
                {
                    gameStateManager.GetLocalPlayerObject ().gameObject.GetComponent<LocalPlayerStats> ().UpdateTime ("Started");
                    gameStateManager.GetLocalPlayerObject
                      ().gameObject.GetComponent<LocalPlayerStats> ().RequestCourseJumpLimit ();
                }
            }
            else if (gameStateManager.TimerIsRunning && !SwitchToOn)
            {
                gameStateManager.SetTimerIsRunning(SwitchToOn);
                gameObject.GetComponent<AudioSource>().PlayOneShot(GameEndSound);
                GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Stop();
                courseCompleteMessage.enabled = true;
                courseCompleteMessage.text = "Course Complete!!\n\nYour time: " + gameStateManager.GetCurrentTime() + "\n\nPress ESC To Quit";

                if (gameStateManager.GetLocalPlayerObject () != null)
                {
                    gameStateManager.GetLocalPlayerObject ().gameObject.GetComponent<LocalPlayerStats> ().UpdateTime (gameStateManager.GetCurrentTime());
                }

                ShootConfetti(other.gameObject);
                SaveLevelCompletion();
                gameStateManager.SetIsCourseComplete(true);
                courseHistoryManager.StoreNewRecord(gameStateManager.GetCourseSeed(), gameStateManager.GetRawTime(), gameStateManager.GetIsCourseFavorited());
                courseHistoryManager.AddRecentlyPlayed(gameStateManager.GetCourseSeed(), gameStateManager.GetRawTime());
                Invoke("EndGame", 7.0f);
            }
        }
    }

    private void SaveLevelCompletion()
    {
        var levelsCompleted = ApplicationManager.LevelsCompleted;
        if (levelsCompleted < ApplicationManager.currentLevel)
        {
            levelsCompleted = ApplicationManager.currentLevel;
            PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
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
    }
}
