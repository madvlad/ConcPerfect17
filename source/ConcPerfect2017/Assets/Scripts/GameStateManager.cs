using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    private int CurrentJump;
    private float CurrentTimerTime;
    public bool TimerIsRunning;
    private List<GameObject> CourseJumpList;
    private int CourseJumpLimit;
    private bool IsCasual;
	
	void Update () {
		if (TimerIsRunning)
        {
            CurrentTimerTime += Time.deltaTime;
            Debug.Log(CurrentTimerTime);
        }
	}

    public void SetTimerIsRunning(bool set)
    {
        if (!IsCasual)
        {
            TimerIsRunning = set;
        }
    }
    public void SetJumpNumer(int num)
    {
        CurrentTimerTime = num;
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
}
