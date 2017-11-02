﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameStateManager : NetworkBehaviour {
    public bool TimerIsRunning;
    public float ConcminationCountdownDuration;
    public GameObject TimerHUDElement;
    public GameObject JumpHUDElement;
    public GameObject JumpNameHUDElement;
    public GameObject EscapeMenuHUDElement;
    public GameObject EscapeMenuSeedElement;
    public GameObject SettingsMenuHUDElement;
	public GameObject PlayerStatsHUDElement;
	public GameObject PlayerInfoHUDElement;
    public GameObject BestTimeHudElement;
    public GameObject BestTimeCrown;
    public GameObject localPlayer;
    public GameObject nicknamePrefab;
    public List<AudioClip> TeamWinSounds;

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
    private List<string> ConcminationWinners = new List<string>();

    [SyncVar]
    private int CourseSeed;

    [SyncVar]
    private int CourseJumpLimit;

    [SyncVar]
    public int CurrentServerLevel;

    [SyncVar]
    public int CurrentGameType;

    [SerializeField]
    private List<string> playerScores;

	[SerializeField]
	private List<string> playerInfo;

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
        ApplicationManager.respawnCount = 0;
        ResetTimer();
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
            bestTimeForCourse = GetComponent<CourseHistoryManager>().GetCurrentCourseRecordBySeed(CourseSeed, ApplicationManager.GetDifficultyLevel());
        }

        if (bestTimeForCourse < float.PositiveInfinity)
        {
            var bestTimeText = BestTimeHudElement.GetComponent<Text>();
            TimeSpan timeSpan = TimeSpan.FromSeconds(bestTimeForCourse);
            string timeString = "";
            if (timeSpan.Hours > 0)
            {
                BestTimeCrown.SetActive(true);
                timeString = "H" + timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
            }
            else
            {
                timeString = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
            }
            bestTimeText.text = timeString;
        }
    }

    void Update() {
        CheckIfPaused();
        CheckIfDisplayStats();
        CheckIfDisplayNicknames();

        if (TimerIsRunning) {
            if (ApplicationManager.GameType != GameTypes.ConcminationGameType) {
                CurrentTimerTime += Time.deltaTime;
            } else {
                CurrentTimerTime -= Time.deltaTime;
            }
            TimeSpan timeSpan = TimeSpan.FromSeconds(CurrentTimerTime);

            if (timeSpan.Hours < 1)
            {
                TimerHUDElement.GetComponent<Text>().text = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
            }
            else
            {
                TimerHUDElement.GetComponent<Text>().text = timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("000");
            }


            if (CurrentTimerTime <= 0 && ApplicationManager.GameType == GameTypes.ConcminationGameType) {
                var RaceStarter = GameObject.FindGameObjectWithTag("RaceStart").GetComponent<SetTimerOnTrigger>();
                RaceStarter.SwitchToOn = false;
                RaceStarter.StopTimer();
                TimerHUDElement.GetComponent<Text>().text = "TIMES UP!";    
            }
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
            GetLocalPlayerObject().GetComponent<FirstPersonDrifter>().SetEscaped(true);
            IsPaused = true;
        } else if (Input.GetButtonDown("Cancel") && IsPaused && !IsDisplayStats) {
            SetPlayerEnabled(true);
            ShowEscapeMenu(false);
            GetLocalPlayerObject().GetComponent<FirstPersonDrifter>().SetEscaped(false);
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

	private void AddTextToPanel(GameObject panel, string label, string text, Color fontColor) {
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        GameObject textObject = new GameObject(label);
        textObject.AddComponent<Text>();
        textObject.GetComponent<Text>().text = text;
        textObject.GetComponent<Text>().font = ArialFont;
		textObject.GetComponent<Text> ().color = fontColor;
        textObject.GetComponent<Text>().material = ArialFont.material;
        textObject.transform.SetParent(panel.transform);
    }

	private void AddHeaderTextToPanel(GameObject panel, string label, string text, int headerLevel) {
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        GameObject textObject = new GameObject(label);
        textObject.AddComponent<Text>();
        textObject.GetComponent<Text>().text = text;
        textObject.GetComponent<Text>().font = ArialFont;
        textObject.GetComponent<Text>().fontStyle = FontStyle.Bold;
		if (headerLevel == 1)
			textObject.GetComponent<Text>().fontSize = 18;
		else
			textObject.GetComponent<Text>().fontSize = 14;
        textObject.GetComponent<Text>().material = ArialFont.material;
        textObject.transform.SetParent(panel.transform);
    }

    public void UpdatePlayerStats() {
        var i = 0;

        foreach (Text row in PlayerInfoHUDElement.GetComponentsInChildren<Text>()) {
			Destroy(row.gameObject);
		}
        if (ApplicationManager.GameType == GameTypes.ConcminationGameType) {
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "In Game", 1);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 1);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 1);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 1);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++, "", 1);
            foreach (string info in playerInfo) {
                i = parseConcminiationModeScores(info, i);
            }
        } else {
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "In Game", 1);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 1);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 1);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 1);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++, "", 1);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "Player", 2);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "Status", 2);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "Current Jump", 2);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "Best Time", 2);
            AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++, "Times Completed", 2);
            foreach (string info in playerInfo) {
                    i = parseRaceModeScores(info, i);
            }
		}
    }

    private int addConcminationPlayerScoreHeaders(int i) {
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "Player", 2);
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "Beacons Captured", 2);
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 2);
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 2);
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++, "", 2);
        return i;
    }

    private int addConcminationTeamScoreHeaders(int i) {
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "Team", 2);
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "Score", 2);
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 2);
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i, "", 2);
        AddHeaderTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++, "", 2);
        return i;
    }

    private int parseRaceModeScores(string info, int i) {
        if (info.Split(';').Length > 5) {
            string pId = info.Split(';')[0];
            string nickname = info.Split(';')[1];
            string status = info.Split(';')[2];
            string jumpNumber = info.Split(';')[3];
            string bestScore = info.Split(';')[4];
            string timesCompleted = info.Split(';')[5];
            Color rowColor = Color.white;
            if (pId.Trim().Equals(GetLocalPlayerObject().GetComponent<NetworkIdentity>().netId.ToString().Trim())) {
                nickname = "*" + nickname;
            }
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i + "nickname", nickname, rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i + "status", status, rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "jumpNumber", jumpNumber, rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "bestScore", bestScore, rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "timesCompleted", timesCompleted, rowColor);
        }
        return i;
    }

    private int parseConcminiationModeScores(string info, int i) {
        if (info.Split(';').Length > 3) {
            string pId = info.Split(';')[0];
            string nickname = info.Split(';')[1];
            string status = info.Split(';')[2];
            string beaconsCaptured = info.Split(';')[3];
            Color rowColor = Color.white;
            if (pId.Trim().Equals(GetLocalPlayerObject().GetComponent<NetworkIdentity>().netId.ToString().Trim())) {
                nickname = "*" + nickname;
            }
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i + "nickname", nickname, rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "beaconsCaptured", beaconsCaptured, rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "emptycell", "", rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "emptycell", "", rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "emptycell", "", rowColor);
        } else if (info.Split(';').Length > 1) {
            i = addConcminationTeamScoreHeaders(i);
            Color rowColor = Color.white;
            string teamName = info.Split(';')[0];
            string beaconsCaptured = info.Split(';')[1];
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i + "teamName", teamName, rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i + "beaconsCaptured", beaconsCaptured, rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "emptycell", "", rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "emptycell", "", rowColor);
            AddTextToPanel(PlayerInfoHUDElement, "InfoRow" + i++ + "emptycell", "", rowColor);
            //Don't print player scors
            //i = addConcminationPlayerScoreHeaders(i);
        }
        return i;
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
        JumpHUDElement.GetComponent<Text>().text = num + " / " + (CourseJumpLimit);

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

    public bool GetIsCourseComplete()
    {
        return IsCourseComplete;
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
        if (ApplicationManager.GameType == GameTypes.RaceGameType || ApplicationManager.GameType == GameTypes.CasualGameType) {
            this.CurrentTimerTime = 0.0f;
        } else if (ApplicationManager.GameType == GameTypes.ConcminationGameType) {
            this.CurrentTimerTime = ConcminationCountdownDuration;
        }
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

    public string GetConcminationEndGameMsg() {
        if (ConcminationWinners.Count == 1) {
            if (ConcminationWinners.Contains(ApplicationManager.GetLocalPlayerObject().GetComponent<Concer>().CurrentTeam)) {
                return "Victory!";
            } else {
                return "Defeat!";
            }
        } else {
            return "Tie Game!";
        }
    }

    public void PlayVictorySong()
    {
        if (ConcminationWinners.Count == 1)
        {
            var audioSource = GameObject.FindGameObjectWithTag("TeamWinSound").GetComponent<AudioSource>();
            switch (ConcminationWinners[0])
            {
                case "Red Rangers":
                    audioSource.PlayOneShot(TeamWinSounds[0], ApplicationManager.sfxVolume);
                    break;
                case "Blue Bandits":
                    audioSource.PlayOneShot(TeamWinSounds[2], ApplicationManager.sfxVolume);
                    break;
                case "Green Gorillas":
                    audioSource.PlayOneShot(TeamWinSounds[1], ApplicationManager.sfxVolume);
                    break;
                case "Yellow Yahoos":
                    audioSource.PlayOneShot(TeamWinSounds[3], ApplicationManager.sfxVolume);
                    break;
            }
        }
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
    public void RpcUpdatePlayerScores(string scores) {
        playerScores = new List<string>(scores.Split('%'));
    }

	[ClientRpc]
	public void RpcUpdatePlayerInfo(string stats) {
		playerInfo = new List<string> (stats.Split ('%'));
	}

    [ClientRpc]
    public void RpcUpdateConcminationWinners(string WinnerMSg) {
        ConcminationWinners.Clear();
        string[] winners = WinnerMSg.Split(',');
        foreach (string w in winners) {
            ConcminationWinners.Add(w);
        }
    }

    [ClientRpc]
    public void RpcUpdateCourseJumpLimit(int CourseJumpLimit) {
        if (!isServer) {
            this.CourseJumpLimit = CourseJumpLimit;
        }
    }

    [ClientRpc]
    public void RpcUpdatePlayerNickname(NetworkInstanceId netId, string nickname) {
        Debug.Log("RPC Called on PlayerNickname Update");
		if (netId == GetLocalPlayerObject().GetComponent<NetworkIdentity>().netId)
            return;
        GameObject networkedPlayer = ClientScene.FindLocalObject(netId);
		if (networkedPlayer != null) {
			GameObject nicknamedGO = Instantiate (nicknamePrefab, networkedPlayer.transform.position + new Vector3 (0, 1, 0), Camera.main.transform.rotation);
			nicknamedGO.GetComponent<Nickname> ().SetPlayerId (netId);
			nicknamedGO.GetComponent<Nickname> ().SetNickname (nickname);
			nicknamedGO.GetComponent<Nickname> ().SetDisplayNickname (IsDisplayNicknames);
		}
    }

    [ClientRpc]
    public void RpcUpdateTeamSkins(NetworkInstanceId netId, int value)
    {
        if (netId == GetLocalPlayerObject().GetComponent<NetworkIdentity>().netId)
            return;
        GameObject networkedPlayer = ClientScene.FindLocalObject(netId);
        if (networkedPlayer != null)
        {
            var networkedDrifter = networkedPlayer.GetComponent<FirstPersonDrifter>();
            if (networkedDrifter != null)
            {
                var networkedModel = networkedDrifter.playerModelRenderer.GetComponent<SkinnedMeshRenderer>();
                networkedModel.material = GameObject.FindGameObjectWithTag("PlayerSkins").GetComponent<PlayerSkinSelectBehavior>().TeamSkins[value];
            }
        }
    }

    [ClientRpc]
    public void RpcUpdatePlayerSkins(NetworkInstanceId netId, int value)
    {
        if (netId == GetLocalPlayerObject().GetComponent<NetworkIdentity>().netId)
            return;
        GameObject networkedPlayer = ClientScene.FindLocalObject(netId);
        if (networkedPlayer != null)
        {
            var networkedDrifter = networkedPlayer.GetComponent<FirstPersonDrifter>();
            var networkedModel = networkedDrifter.playerModelRenderer.GetComponent<SkinnedMeshRenderer>();
            networkedModel.material = GameObject.FindGameObjectWithTag("PlayerSkins").GetComponent<PlayerSkinSelectBehavior>().playerSkins[value];
        }
    }

    [ClientRpc]
    public void RpcResetConcminationAssets()
    {
        ResetTimer();
    }
}
