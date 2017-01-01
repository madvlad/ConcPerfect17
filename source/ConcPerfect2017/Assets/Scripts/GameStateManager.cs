using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    private int CurrentJump;
    private float CurrentTimerTime;
    private bool TimerIsRunning;
    private List<GameObject> CourseJumpList;
    private int CourseJumpLimit;
    private bool IsCasual;
	
	void Update () {
		if (TimerIsRunning)
        {
            CurrentTimerTime += Time.deltaTime;
        }
	}

    void StartTimer()
    {
        if (!IsCasual)
        {
            TimerIsRunning = true;
        }
    }

    void StopTimer()
    {
        if (!IsCasual)
        {
            TimerIsRunning = false;
        }
    }

    void SetJumpNumer(int num)
    {
        CurrentTimerTime = num;
    }

    void SetCourseJumps(List<GameObject> CourseJumpList)
    {
        this.CourseJumpList = CourseJumpList;
        CourseJumpLimit = CourseJumpList.Count;
    }

    void SetCasual()
    {
        this.IsCasual = true;
    }
}
