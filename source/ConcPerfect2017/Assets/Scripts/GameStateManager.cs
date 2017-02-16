using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameStateManager : NetworkBehaviour {
    public bool TimerIsRunning;
    public GameObject TimerHUDElement;
    public GameObject JumpHUDElement;
    public GameObject JumpNameHUDElement;
    public GameObject EscapeMenuHUDElement;
    public GameObject EscapeMenuSeedElement;
    public GameObject SettingsMenuHUDElement;
    public GameObject PlayerStatsHUDElement;
    public GameObject BestTimeHudElement;
    public GameObject localPlayer;
    public GameObject nicknamePrefab;

    // Server Objects
    public GameServerManager gameServerManager;

    private int GameType;
    private float CurrentTimerTime;
    private List<GameObject> CourseJumpList;
    private int CurrentJumpNumber;
    private bool IsCasual;
    private bool IsPaused = false;
    private bool IsDisplayStats = false;
    private bool IsDisplayNicknames = true;
    private bool IsCourseComplete = false;
    private bool IsCourseFavorited = false;

    [SyncVar]
    private int CourseSeed;

    [SyncVar]
    private int CourseJumpLimit;

    [SyncVar]
    public int CurrentServerLevel;

    [SyncVar]
    public int CurrentGameType;

    [SerializeField]
    private List<string> playerStats;

    void Start() {
        GameType = ApplicationManager.GameType;

        if (isServer)
        {
            gameServerManager = GameObject.FindGameObjectWithTag("GameServerManager").GetComponent<GameServerManager>();
            CurrentServerLevel = ApplicationManager.currentLevel;
            CurrentGameType = ApplicationManager.GameType;
        }
        else
        {
            ApplicationManager.currentLevel = CurrentServerLevel;
            ApplicationManager.GameType = CurrentGameType;
        }

        SetBestTime();
    }

    private void SetBestTime()
    {
        float bestTimeForCourse;

        if(CourseSeed == 0)
        {
            bestTimeForCourse = GetComponent<CourseHistoryManager>().GetCurrentCourseRecordByLevel(ApplicationManager.currentLevel);
        }
        else
        {
            bestTimeForCourse = GetComponent<CourseHistoryManager>().GetCurrentCourseRecordBySeed(CourseSeed);
        }
        var bestTimeText = BestTimeHudElement.GetComponent<Text>();
        TimeSpan timeSpan = TimeSpan.FromSeconds(bestTimeForCourse);
        var timeString = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
        bestTimeText.text = "Best time: " + timeString;
    }

    void Update() {
        CheckIfPaused();
        CheckIfDisplayStats();
        CheckIfDisplayNicknames();

        if (TimerIsRunning) {
            CurrentTimerTime += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(CurrentTimerTime);
            TimerHUDElement.GetComponent<Text>().text = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
        }
    }

    private void CheckIfDisplayNicknames() {
        if (Input.GetButtonDown("DisplayNames")) {
            IsDisplayNicknames = !IsDisplayNicknames;
            if (IsDisplayNicknames) {
                GetLocalPlayerObject().GetComponent<LocalPlayerStats>().RequestPlayerNicknames();
            } else {
                foreach (GameObject nickname in GameObject.FindGameObjectsWithTag("Nickname")) {
                    Destroy(nickname);
                }
            }
        }
    }

    private bool CheckIfDisplayStats() {
        if (Input.GetButtonDown("Tab") && !IsPaused && !IsDisplayStats) {
            IsDisplayStats = true;
            UpdatePlayerStats();
            ShowPlayerStats(true);
        } else if (Input.GetButton("Tab") && IsDisplayStats) {
            UpdatePlayerStats();
        } else if (Input.GetButtonUp("Tab") && IsDisplayStats) {
            ShowPlayerStats(false);
        }

        return IsDisplayStats;
    }

    private bool CheckIfPaused() {
        if (Input.GetButtonDown("Cancel") && !IsPaused && !IsDisplayStats) {
            SetPlayerEnabled(false);
            ShowEscapeMenu(true);
            IsPaused = true;
        } else if (Input.GetButtonDown("Cancel") && IsPaused && !IsCourseComplete && !IsDisplayStats) {
            SetPlayerEnabled(true);
            ShowEscapeMenu(false);
            IsPaused = false;
        }
        return IsPaused;
    }

    public string GetCurrentTime() {
        return TimerHUDElement.GetComponent<Text>().text;
    }

    public void SetPlayerEnabled(bool enabled) {
        IsPaused = !enabled;

        var player = GetLocalPlayerObject();
        var camera = Camera.main;
        player.GetComponent<MouseLook>().enabled = enabled;
        camera.GetComponent<LockMouse>().enabled = enabled;
        camera.GetComponent<MouseLook>().enabled = enabled;

        Cursor.lockState = CursorLockMode.None;
    } 


    public void LockPlayer(bool locked) {
        IsPaused = !enabled;

        var player = GetLocalPlayerObject();
        var camera = Camera.main;

        player.GetComponent<Rigidbody>().mass = enabled ? 1 : float.MaxValue;
        player.GetComponent<FirstPersonDrifter>().enabled = enabled;
        player.GetComponent<MouseLook>().enabled = enabled;
        player.GetComponent<Concer>().enabled = enabled;
        player.GetComponent<Footsteps>().enabled = enabled;
        player.GetComponent<ImpactReceiver>().enabled = enabled;
        camera.GetComponent<LockMouse>().enabled = enabled;
        camera.GetComponent<MouseLook>().enabled = enabled;

        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowEscapeMenu(bool show) {
        if (!show) {
            SettingsMenuHUDElement.SetActive(show);
        }

        EscapeMenuHUDElement.SetActive(show);
        Cursor.visible = show;
    }

    public bool IsDisplayingStats() 
    {
        return IsDisplayStats;
    }

    private void AddTextToPanel(GameObject panel, string label, string text) {
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        GameObject textObject = new GameObject(label);
        textObject.AddComponent<Text>();
        textObject.GetComponent<Text>().text = text;
        textObject.GetComponent<Text>().font = ArialFont;
        textObject.GetComponent<Text>().material = ArialFont.material;
        textObject.transform.SetParent(panel.transform);
    }

    private void AddHeaderTextToPanel(GameObject panel, string label, string text) {
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        GameObject textObject = new GameObject(label);
        textObject.AddComponent<Text>();
        textObject.GetComponent<Text>().text = text;
        textObject.GetComponent<Text>().font = ArialFont;
        textObject.GetComponent<Text>().fontStyle = FontStyle.Bold;
        textObject.GetComponent<Text>().fontSize = 18;
        textObject.GetComponent<Text>().material = ArialFont.material;
        textObject.transform.SetParent(panel.transform);
    }

    public void UpdatePlayerStats() {
        foreach (Text row in PlayerStatsHUDElement.GetComponentsInChildren<Text>()) {
            Destroy(row.gameObject);
        }

        var i = 0;
        AddHeaderTextToPanel(PlayerStatsHUDElement, "Row" + i, "Player");
        AddHeaderTextToPanel(PlayerStatsHUDElement, "Row" + i, "Current Jump");
        AddHeaderTextToPanel(PlayerStatsHUDElement, "Row" + i++, "Course Status");
        foreach (string stat in playerStats) {
            if (stat.Split(';').Length > 2) {
                string playerId = stat.Split(';')[0];
                string courseTime = stat.Split(';')[1];
                string jumpNumber = stat.Split(';')[2];
                AddTextToPanel(PlayerStatsHUDElement, "Row" + i + playerId, playerId);
                AddTextToPanel(PlayerStatsHUDElement, "Row" + i + courseTime, courseTime);
                AddTextToPanel(PlayerStatsHUDElement, "Row" + i++ + jumpNumber, jumpNumber);
            }
        }
    }

    public void ShowPlayerStats(bool show) {
        if (show) {
            PlayerStatsHUDElement.SetActive(show);
        }

        if (!show) {
            IsDisplayStats = false;
            PlayerStatsHUDElement.SetActive(show);
        }
    }

    public void SetTimerIsRunning(bool set) {
        if (!IsCasual) {
            TimerIsRunning = set;
        }
    }

    public void SetJumpNumber(int num) {
        this.CurrentJumpNumber = num;
        JumpHUDElement.GetComponent<Text>().text = "Jump: " + num + " / " + (CourseJumpLimit);

        if (GetLocalPlayerObject() != null) {
            GetLocalPlayerObject().gameObject.GetComponent<LocalPlayerStats>().UpdateJump(CurrentJumpNumber);
        }
    }

    public void SetJumpName(string name) {
        JumpNameHUDElement.GetComponent<Text>().text = name;
    }

    public int GetCourseJumpLimit() {
        return CourseJumpLimit;
    }

    public int GetCurrentJumpNumber() {
        return CurrentJumpNumber;
    }

    public void SetCourseJumps(List<GameObject> CourseJumpList) {
        this.CourseJumpList = CourseJumpList;
        CourseJumpLimit = CourseJumpList.Count;
    }

    public void SetCasual() {
        this.IsCasual = true;
    }

    public void SetCourseJumpLimit(int limit) {
        this.CourseJumpLimit = limit;
    }

    public void SetIsCourseComplete(bool isComplete) {
        IsCourseComplete = isComplete;
    }

    public void SetJumpSeed(int seed) {
        this.CourseSeed = seed;
        EscapeMenuSeedElement.GetComponent<Text>().text = "Seed: " + seed;
    }

    public int GetCourseSeed()
    {
        return CourseSeed;
    }

    public void ResetTimer()
    {
        this.CurrentTimerTime = 0.0f;
    }

    public float GetRawTime()
    {
        return this.CurrentTimerTime;
    }

    public bool IsGamePause()
    {
        return IsPaused;
    }

    public bool GetIsCourseFavorited()
    {
        return IsCourseFavorited;
    }

    public GameObject GetLocalPlayerObject() {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject playerObject = null;
        foreach (GameObject obj in playerObjects) {
            if (obj.GetComponent<NetworkIdentity>().isLocalPlayer) {
                playerObject = obj;
            }
        }

        return playerObject;
    }

    [ClientRpc]
    public void RpcUpdatePlayerStats(string stats) {
        playerStats = new List<string>(stats.Split('%'));
    }

    [ClientRpc]
    public void RpcUpdateCourseJumpLimit(int CourseJumpLimit) {
        if (!isServer) {
            this.CourseJumpLimit = CourseJumpLimit;
        }
    }

    [ClientRpc]
    public void RpcUpdatePlayerNickname(NetworkInstanceId netId, string nickname) {
		if (netId == GetLocalPlayerObject().GetComponent<NetworkIdentity>().netId)
            return;
        GameObject networkedPlayer = ClientScene.FindLocalObject(netId);
        GameObject nicknamedGO = Instantiate(nicknamePrefab, networkedPlayer.transform.position + new Vector3(0, 1, 0), Camera.main.transform.rotation);
        nicknamedGO.GetComponent<Nickname>().SetPlayerId(netId);
        nicknamedGO.GetComponent<Nickname>().SetNickname(nickname);
        nicknamedGO.GetComponent<Nickname>().SetDisplayNickname(IsDisplayNicknames);
    }
}
