﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public bool TimerIsRunning;
    public GameObject TimerHUDElement;
    public GameObject JumpHUDElement;
    public GameObject EscapeMenuHUDElement;
    public GameObject EscapeMenuSeedElement;
    public GameObject SettingsMenuHUDElement;
	public GameObject PlayerStatsHUDElement;
	public NetworkPlayerStats networkPlayerStats;

    private float CurrentTimerTime;
    private List<GameObject> CourseJumpList;
    private int CourseJumpLimit;
	private int CurrentJumpNumber;
    private bool IsCasual;
    private bool IsPaused = false;
	private bool IsDisplayStats = false;
    private bool IsCourseComplete = false;
    private int CourseSeed;

	void Start()
	{
		networkPlayerStats = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<NetworkPlayerStats> ();
	}

	void Update ()
    {
        if (CheckIfPaused())
        {
            return;
        }

		CheckIfDisplayStats();

        if (TimerIsRunning)
        {
            CurrentTimerTime += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(CurrentTimerTime);
            TimerHUDElement.GetComponent<Text>().text = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
        }
    }


	// TODO - Update Stats while tab is held, currently stats only updated after open and close.
	private bool CheckIfDisplayStats()
	{
		if (Input.GetButtonDown ("Tab") && !IsPaused && !IsDisplayStats) {
			IsDisplayStats = true;
			ShowPlayerStats (true);
		} else if (Input.GetButtonUp("Tab") && IsDisplayStats) {
			ShowPlayerStats (false);
		}

		return IsDisplayStats;
	}

    private bool CheckIfPaused()
    {
		if (Input.GetButtonDown("Cancel") && !IsPaused && !IsDisplayStats)
        {
            SetPlayerEnabled(false);
            ShowEscapeMenu(true);
            IsPaused = true;
        }
		else if (Input.GetButtonDown("Cancel") && IsPaused && !IsCourseComplete && !IsDisplayStats)
        {
            SetPlayerEnabled(true);
            ShowEscapeMenu(false);
            IsPaused = false;
        }
        return IsPaused;
    }

    public string GetCurrentTime()
    {
        return TimerHUDElement.GetComponent<Text>().text;
    }

    public void SetPlayerEnabled(bool enabled)
    {
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

    public void ShowEscapeMenu(bool show)
    {
        if (!show)
        {
            SettingsMenuHUDElement.SetActive(show);
        }

        EscapeMenuHUDElement.SetActive(show);
        Cursor.visible = show;
    }

	public bool IsDisplayingStats()
	{
		return IsDisplayStats;
	}

	// TODO - Style each Row and add Table Header
	public void ShowPlayerStats(bool show)
	{
		if (show) 
		{
			foreach (Text row in PlayerStatsHUDElement.GetComponentsInChildren<Text>()) {
				Destroy (row.gameObject);
			}

			Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

			var i = 0;
			foreach (string stat in networkPlayerStats.GetPlayerStats()) {
				GameObject newRowGO = new GameObject ("Player " + i++);
				newRowGO.AddComponent<Text>();
				newRowGO.GetComponent<Text> ().text = stat;
				newRowGO.GetComponent<Text>().font = ArialFont;
				newRowGO.GetComponent<Text>().material = ArialFont.material;
				newRowGO.transform.SetParent(PlayerStatsHUDElement.transform);
			}

			PlayerStatsHUDElement.SetActive (show);
		}

		if (!show) {
			IsDisplayStats = false;
			PlayerStatsHUDElement.SetActive (show);
		}
	}

    public void SetTimerIsRunning(bool set)
    {
        if (!IsCasual)
        {
            TimerIsRunning = set;
        }
    }

    public void SetJumpNumber(int num)
    {
		this.CurrentJumpNumber = num;
        JumpHUDElement.GetComponent<Text>().text = "Jump: " + num + " / " + (CourseJumpLimit);
    }

	public int GetCourseJumpLimit()
	{
		return CourseJumpLimit;
	}

	public int GetCurrentJumpNumber()
	{
		return CurrentJumpNumber;
	}
		
    public void SetCourseJumps(List<GameObject> CourseJumpList)
    {
        this.CourseJumpList = CourseJumpList;
        CourseJumpLimit = CourseJumpList.Count;
    }

    public void SetCasual()
    {
        this.IsCasual = true;
    }

    public void SetCourseJumpLimit(int limit)
    {
        this.CourseJumpLimit = limit;
    }

    public void SetIsCourseComplete(bool isComplete)
    {
        IsCourseComplete = isComplete;
    }

    public void SetJumpSeed(int seed)
    {
        this.CourseSeed = seed;
        EscapeMenuSeedElement.GetComponent<Text>().text = "Seed: " + seed;
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
