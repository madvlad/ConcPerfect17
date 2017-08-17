using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHistoryTable : MonoBehaviour {

    public GameObject EntryPrefab;
    public GameObject GridElement;

	void Start ()
    {
        GetRecordsFor("CourseRecords");
    }

    private void GetRecordsFor(string key)
    {
        var manager = GetComponent<CourseHistoryManager>();
        var list = manager.GetSavedRecords(key);
        int idx = 1;

        foreach (var entry in list)
        {
            if (entry.TimeCompleted < float.PositiveInfinity)
            {
                var entryInstance = Instantiate(EntryPrefab);
                var entryManager = entryInstance.GetComponent<HistoryEntryManager>();

                TimeSpan timeSpan = TimeSpan.FromSeconds(entry.TimeCompleted);
                string timeString = "";

                if (timeSpan.Hours > 0)
                {
                    timeString = "H" + timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
                }
                else
                {
                    timeString = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
                }

                if (entry.CourseSeed == 0)
                {
                    entryManager.SetFields(idx.ToString(), entry.CourseName, timeString, entry.IsFavorited, "");
                }
                else
                {
                    entryManager.SetFields(idx.ToString(), entry.CourseSeed.ToString(), timeString, entry.IsFavorited, entry.DifficultyLevel.ToString());
                }

                entryInstance.transform.SetParent(GridElement.transform);
                entryInstance.transform.localScale = new Vector3(1,1,1);
                idx++;
            }
        }
    }

    public void ShowAll()
    {
        foreach (Transform child in GridElement.transform)
        {
            Destroy(child.gameObject);
        }

        GetRecordsFor("RecentPlayed");
    }

    public void ShowBest()
    {
        foreach (Transform child in GridElement.transform)
        {
            Destroy(child.gameObject);
        }

        GetRecordsFor("CourseRecords");
    }

	void Update () {
		
	}
}
