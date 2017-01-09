using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public bool TimerIsRunning;
    public GameObject TimerHUDElement;
    public GameObject JumpHUDElement;

    private int CurrentJump;
    private float CurrentTimerTime;
    private List<GameObject> CourseJumpList;
    private int CourseJumpLimit;
    private bool IsCasual;

	void Update () {
		if (TimerIsRunning)
        {
            CurrentTimerTime += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(CurrentTimerTime);
            TimerHUDElement.GetComponent<Text>().text = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
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
}
