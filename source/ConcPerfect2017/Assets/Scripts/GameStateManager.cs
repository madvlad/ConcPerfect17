using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public bool TimerIsRunning;
    public GameObject TimerHUDElement;
    public GameObject JumpHUDElement;
    public GameObject EscapeMenuHUDElement;

    private int CurrentJump;
    private float CurrentTimerTime;
    private List<GameObject> CourseJumpList;
    private int CourseJumpLimit;
    private bool IsCasual;
    private bool IsPaused = false;
    private bool IsCourseComplete = false;

	void Update ()
    {
        if (CheckIfPaused())
        {
            return;
        }

        if (TimerIsRunning)
        {
            CurrentTimerTime += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(CurrentTimerTime);
            TimerHUDElement.GetComponent<Text>().text = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
        }
    }

    private bool CheckIfPaused()
    {
        if (Input.GetButtonDown("Cancel") && !IsPaused)
        {
            SetPlayerEnabled(false);
            return true;
        }
        else if (Input.GetButtonDown("Cancel") && IsPaused && !IsCourseComplete)
        {
            SetPlayerEnabled(true);
            return false;
        }
        return IsPaused;
    }

    public void SetPlayerEnabled(bool enabled)
    {
        IsPaused = !enabled;
        Cursor.lockState = CursorLockMode.Locked;
        EscapeMenuHUDElement.SetActive(!enabled);

        var player = GameObject.FindGameObjectWithTag("Player");
        var camera = Camera.main;

        player.GetComponent<Rigidbody>().mass = enabled ? 1 : float.MaxValue;
        player.GetComponent<FirstPersonDrifter>().enabled = enabled;
        player.GetComponent<MouseLook>().enabled = enabled;
        player.GetComponent<Concer>().enabled = enabled;
        player.GetComponent<Footsteps>().enabled = enabled;
        player.GetComponent<ImpactReceiver>().enabled = enabled;
        camera.GetComponent<LockMouse>().enabled = enabled;
        camera.GetComponent<MouseLook>().enabled = enabled;
        Cursor.visible = !enabled;
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
        CurrentJump = num;
        JumpHUDElement.GetComponent<Text>().text = "Jump: " + num + " / " + (CourseJumpLimit);
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
}
