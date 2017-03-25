using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalDetector : MonoBehaviour {
    public GameObject MedalImage;
    public GameObject Qualifier;

    public Sprite goldMedal;
    public Sprite silverMedal;
    public Sprite bronzeMedal;

    public int Level;

	void Start () {
        var historyManager = new CourseHistoryManager();
        var qualifier = Qualifier.GetComponent<TimeQualifier>();
        var currentRecord = historyManager.GetCurrentCourseRecordByLevel(Level);
        var medal = qualifier.CheckTime(currentRecord, Level);
        SetMedal(medal);
	}

    private void SetMedal(int medal)
    {
        var image = MedalImage.GetComponent<Image>();

        switch (medal)
        {
            case 1:
                image.sprite = bronzeMedal;
                return;
            case 2:
                image.sprite = silverMedal;
                return;
            case 3:
                image.sprite = goldMedal;
                return;
        }
    }
}
