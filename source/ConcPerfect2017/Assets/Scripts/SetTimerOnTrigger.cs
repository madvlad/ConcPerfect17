using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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
            if (!gameStateManager.TimerIsRunning && SwitchToOn && !gameStateManager.GetIsCourseComplete())
            {
                gameStateManager.SetTimerIsRunning(SwitchToOn);
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                startLabel.GetComponent<MeshRenderer>().enabled = false;

                if (gameStateManager.GetLocalPlayerObject () != null)
                {
					gameStateManager.GetLocalPlayerObject ().gameObject.GetComponent<LocalPlayerStats> ().UpdateStatus ("Started");
                    gameStateManager.GetLocalPlayerObject ().gameObject.GetComponent<LocalPlayerStats> ().RequestCourseJumpLimit ();
                }

                other.GetComponent<MultiplayerChatScript>().SendStartMessage();
            }
            else if (gameStateManager.TimerIsRunning && !SwitchToOn)
            {
                gameStateManager.SetTimerIsRunning(SwitchToOn);
                gameObject.GetComponent<AudioSource>().PlayOneShot(GameEndSound, ApplicationManager.sfxVolume);
                GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Stop();
                courseCompleteMessage.enabled = true;
                courseCompleteMessage.text = "Course Complete!!\n\nYour time: " + gameStateManager.GetCurrentTime() + "\n\nPress ESC To Quit";

                var reward = other.GetComponent<TimeQualifier>().CheckTime(gameStateManager.GetRawTime(), ApplicationManager.currentLevel);
                other.GetComponent<MultiplayerChatScript>().WriteLocalMessage(DisplayRewardMessage(reward));

                if (gameStateManager.GetLocalPlayerObject () != null)
                {
                    gameStateManager.GetLocalPlayerObject ().gameObject.GetComponent<LocalPlayerStats> ().UpdateTime (gameStateManager.GetCurrentTime());
                }

                ShootConfetti(other.gameObject);
                if (reward != -1)
                {
                    SaveLevelCompletion();
                }
                gameStateManager.SetIsCourseComplete(true);

                TimeSpan timeSpan = TimeSpan.FromSeconds(gameStateManager.GetRawTime());
                string timeString = "";
                if (timeSpan.Hours > 0)
                {
                    timeString += "H" + timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
                }
                else
                {
                    timeString += timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
                }

                if (courseHistoryManager.StoreNewRecord(gameStateManager.GetCourseSeed(), gameStateManager.GetRawTime(), gameStateManager.GetIsCourseFavorited(), ApplicationManager.GetDifficultyLevel()))
                {
                    GameObject.FindGameObjectWithTag("TheCrown").SetActive(true);
                    GameObject.FindGameObjectWithTag("BestTime").GetComponent<Text>().text = timeString;
                    other.GetComponent<MultiplayerChatScript>().SendRecordFinishMessage(timeString);
                }
                else
                {
                    other.GetComponent<MultiplayerChatScript>().SendFinishMessage(timeString);
                }

                courseHistoryManager.AddRecentlyPlayed(gameStateManager.GetCourseSeed(), gameStateManager.GetRawTime(), ApplicationManager.GetDifficultyLevel());

                if (ApplicationManager.GameType == GameTypes.RaceGameType)
                {
                    other.GetComponent<FirstPersonDrifter>().CmdFreezeAll(true);
                    Invoke("Unfreeze", 5.0f);
                }

                Invoke("EndGame", 7.0f);
            }
        }
    }

    void Unfreeze()
    {
        var oneLuckyGuy = GetLocalPlayerObject();
        oneLuckyGuy.GetComponent<FirstPersonDrifter>().CmdFreezeAll(false);
    }

    private string DisplayRewardMessage(int reward)
    {
        switch(reward)
        {
            case 0:
                return "Nice, you qualified for the next course!";
            case 1:
                return "Sweet! You earned a bronze medal!";
            case 2:
                return "Great work! You earned a silver medal!";
            case 3:
                return "Awesome Concin! You earned a gold medal!";
            default:
                return "Sorry, you did not qualify for the next course.";
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
