using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Steamworks;

public class SetTimerOnTrigger : MonoBehaviour {
    public GameObject gameManager;
    public GameObject startLabel;
    public bool SwitchToOn;
    public AudioClip GameEndSound;
    public ParticleSystem confetti1;
    public ParticleSystem confetti2;
    public ParticleSystem confetti3;
    public bool EnableTrigger = true;

    private GameStateManager gameStateManager;
    private CourseHistoryManager courseHistoryManager;
    private Text courseCompleteMessage;

    void Start()
    {
        gameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        courseHistoryManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CourseHistoryManager>();
        courseCompleteMessage = GameObject.FindGameObjectWithTag("CourseEndText").GetComponent<Text>();
    }

    public void StartTimer() {
        GameObject player = ApplicationManager.GetLocalPlayerObject();
        if (!gameStateManager.TimerIsRunning && SwitchToOn && !gameStateManager.GetIsCourseComplete()) {
            gameStateManager.SetTimerIsRunning(SwitchToOn);
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            if (startLabel != null)
                    startLabel.GetComponent<MeshRenderer>().enabled = false;

            if (gameStateManager.GetLocalPlayerObject() != null) {
                gameStateManager.GetLocalPlayerObject().gameObject.GetComponent<LocalPlayerStats>().UpdateStatus("Started");
                gameStateManager.GetLocalPlayerObject().gameObject.GetComponent<LocalPlayerStats>().RequestCourseJumpLimit();
            }

            player.GetComponent<MultiplayerChatScript>().SendStartMessage();
        }
    }

    public void StopTimer() {
        GameObject player = ApplicationManager.GetLocalPlayerObject();
        if (gameStateManager.TimerIsRunning && !SwitchToOn) {
            gameStateManager.SetTimerIsRunning(SwitchToOn);
            gameObject.GetComponent<AudioSource>().PlayOneShot(GameEndSound, ApplicationManager.sfxVolume);
            GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Stop();

            if (ApplicationManager.GameType == GameTypes.ConcminationGameType) {
                courseCompleteMessage.enabled = true;
                courseCompleteMessage.text = gameStateManager.GetConcminationEndGameMsg() + "\n\nPress ESC To Quit";
            }  else {
                courseCompleteMessage.enabled = true;
                courseCompleteMessage.text = "Course Complete!!\n\nYour time: " + gameStateManager.GetCurrentTime() + "\n\nPress ESC To Quit";
                var reward = player.GetComponent<TimeQualifier>().CheckTime(gameStateManager.GetRawTime(), ApplicationManager.currentLevel);

                if (ApplicationManager.currentLevel != 0) {
                    player.GetComponent<MultiplayerChatScript>().WriteLocalMessage(DisplayRewardMessage(reward));
                }

                SetCompletionAchievement(reward, ApplicationManager.currentLevel);

                if (gameStateManager.GetLocalPlayerObject() != null) {
                    gameStateManager.GetLocalPlayerObject().gameObject.GetComponent<LocalPlayerStats>().UpdateTime(gameStateManager.GetCurrentTime());
                }

                ShootConfetti(player.gameObject);
                if (reward != -1) {
                    SaveLevelCompletion();
                }
                gameStateManager.SetIsCourseComplete(true);

                TimeSpan timeSpan = TimeSpan.FromSeconds(gameStateManager.GetRawTime());
                string timeString = "";
                if (timeSpan.Hours > 0) {
                    timeString += "H" + timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
                } else {
                    timeString += timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
                }

                SetSpecialLevelTimeAchievements(timeSpan);

                if (courseHistoryManager.StoreNewRecord(gameStateManager.GetCourseSeed(), gameStateManager.GetRawTime(), gameStateManager.GetIsCourseFavorited(), ApplicationManager.GetDifficultyLevel())) {
                    GameObject.FindGameObjectWithTag("TheCrown").SetActive(true);
                    GameObject.FindGameObjectWithTag("BestTime").GetComponent<Text>().text = timeString;
                    player.GetComponent<MultiplayerChatScript>().SendRecordFinishMessage(timeString);
                } else {
                    player.GetComponent<MultiplayerChatScript>().SendFinishMessage(timeString);
                }

                courseHistoryManager.AddRecentlyPlayed(gameStateManager.GetCourseSeed(), gameStateManager.GetRawTime(), ApplicationManager.GetDifficultyLevel());

                var totalCoursesCompleted = courseHistoryManager.GetSavedRecords("RecentPlayed");
                SetTotalCompletedAchievement(totalCoursesCompleted.Count);

                if (!ApplicationManager.IsLAN) {
                    if (SteamManager.Initialized) {
                        if (PlayerPrefs.HasKey("OnlineModeRecord")) {
                            var winRecord = PlayerPrefs.GetInt("OnlineModeRecord");
                            winRecord++;
                            if (winRecord >= 10) {
                                SteamUserStats.SetAchievement("ACHIEVEMENT_SOCIAL_CONCER");
                            }
                            PlayerPrefs.SetInt("OnlineModeRecord", winRecord);
                        } else {
                            PlayerPrefs.SetInt("OnlineModeRecord", 1);
                        }
                    }
                }

                if (ApplicationManager.GameType == GameTypes.RaceGameType) {
                    if (SteamManager.Initialized) {
                        if (PlayerPrefs.HasKey("RaceModeWinRecord")) {
                            var winRecord = PlayerPrefs.GetInt("RaceModeWinRecord");
                            winRecord++;
                            if (winRecord >= 10) {
                                SteamUserStats.SetAchievement("ACHIEVEMENT_STREET_CONCER");
                            }
                            PlayerPrefs.SetInt("RaceModeWinRecord", winRecord);
                        } else {
                            PlayerPrefs.SetInt("RaceModeWinRecord", 1);
                        }
                        SteamUserStats.SetAchievement("ACHIEVEMENT_WIN_RACE");
                    }
                    player.GetComponent<FirstPersonDrifter>().CmdFreezeAll(true);
                    Invoke("Unfreeze", 5.0f);
                }
            }

            Invoke("EndGame", 7.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EnableTrigger) {
            if (other.CompareTag("Player")) {
                if (!gameStateManager.TimerIsRunning && SwitchToOn && !gameStateManager.GetIsCourseComplete()) {
                    StartTimer();
                } else if (gameStateManager.TimerIsRunning && !SwitchToOn) {
                    StopTimer();
                }
            }
        }
    }

    private void SetSpecialLevelTimeAchievements(TimeSpan timeSpan)
    {
        if (ApplicationManager.GameType == GameTypes.TutorialGameType && timeSpan.TotalSeconds <= 8)
        {
            SteamUserStats.SetAchievement("ACHIEVEMENT_COOKIE_BOY");
        }
        if (ApplicationManager.currentLevel == 1 && timeSpan.TotalSeconds <= 12)
        {
            SteamUserStats.SetAchievement("ACHIEVEMENT_RESET_CONC");
        }
        if (ApplicationManager.currentLevel == 5 && timeSpan.TotalMinutes <= 5)
        {
            SteamUserStats.SetAchievement("ACHIEVEMENT_DEVS");
        }
    }

    void SetTotalCompletedAchievement(int count)
    {
        if (SteamManager.Initialized)
        {
            if (count >= 10)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_10_JUMPS");
            }
            if (count >= 25)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_25_JUMPS");
            }
            if (count >= 50)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_50_JUMPS");
            }
            if (count >= 100)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_100_JUMPS");
            }
            if (count >= 500)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_500_JUMPS");
            }
        }
    }

    void SetCompletionAchievement(int reward, int levelNumber)
    {
        if (SteamManager.Initialized)
        {
            if (levelNumber == 1 && reward == 3)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_LVL1_GOLD");
                Debug.Log("Set achievement ACHIEVEMENT_LVL1_GOLD");
            }
            if (levelNumber == 2 && reward == 3)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_LVL2_GOLD");
                Debug.Log("Set achievement ACHIEVEMENT_LVL2_GOLD");
            }
            if (levelNumber == 3 && reward == 3)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_LVL3_GOLD");
                Debug.Log("Set achievement ACHIEVEMENT_LVL3_GOLD");
            }
            if (levelNumber == 4 && reward == 3)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_LVL4_GOLD");
                Debug.Log("Set achievement ACHIEVEMENT_LVL4_GOLD");
            }
            if (levelNumber == 5 && reward == 3)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_LVL5_GOLD");
                Debug.Log("Set achievement ACHIEVEMENT_LVL5_GOLD");
            }
            if (levelNumber == 6 && reward == 3)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_LVL6_GOLD");
                Debug.Log("Set achievement ACHIEVEMENT_LVL6_GOLD");
            }
            if (levelNumber == 7 && reward == 3)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_LVL7_GOLD");
                Debug.Log("Set achievement ACHIEVEMENT_LVL7_GOLD");
            }
            if (levelNumber == 8 && reward == 3)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_LVL8_GOLD");
                Debug.Log("Set achievement ACHIEVEMENT_LVL8_GOLD");
            }
            if (levelNumber == 5)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_CAMPAIGN_CLEARED");
                Debug.Log("Set achievement ACHIEVEMENT_CAMPAIGN_CLEARED");
            }

            bool completed1 = false;
            bool completed2 = false;
            bool completed3 = false;
            bool completed4 = false;
            bool completed5 = false;

            SteamUserStats.GetAchievement("ACHIEVEMENT_LVL1_GOLD", out completed1);
            SteamUserStats.GetAchievement("ACHIEVEMENT_LVL2_GOLD", out completed2);
            SteamUserStats.GetAchievement("ACHIEVEMENT_LVL3_GOLD", out completed3);
            SteamUserStats.GetAchievement("ACHIEVEMENT_LVL4_GOLD", out completed4);
            SteamUserStats.GetAchievement("ACHIEVEMENT_LVL5_GOLD", out completed5);

            if (completed1 && completed2 && completed3 && completed4 && completed5)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_PRO");
                Debug.Log("Set achievement ACHIEVEMENT_PRO");
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
