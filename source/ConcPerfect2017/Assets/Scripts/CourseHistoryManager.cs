using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseHistoryManager : MonoBehaviour {

    private CourseHistoryEntry previousBestTime;

    public float GetCurrentCourseRecord(int CourseSeed)
    {
        var currentRecords = GetCurrentRecords();

        foreach (var record in currentRecords)
        {
            if (record.CourseSeed == CourseSeed)
            {
                return record.TimeCompleted;
            }
        }
        return 0.0f;
    }

    public void StoreNewRecord(int CourseSeed, float TimeCompleted, bool IsFavorited)
    {
        var entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, IsFavorited = IsFavorited };
        var currentRecords = GetCurrentRecords();
        
        if (IsFirstRecord(currentRecords, entry))
        {
            currentRecords.Add(entry);
            SerializeAndSave(currentRecords);
        }
        else if (IsBestTime(currentRecords, entry))
        {
            currentRecords.Remove(previousBestTime);
            currentRecords.Add(entry);
            SerializeAndSave(currentRecords);
        }
    }

    private bool IsFirstRecord(List<CourseHistoryEntry> currentRecords, CourseHistoryEntry entry)
    {
        foreach (var record in currentRecords)
        {
            if (record.CourseSeed == entry.CourseSeed)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsBestTime(List<CourseHistoryEntry> currentRecords, CourseHistoryEntry entry)
    {
        foreach (var record in currentRecords)
        {
            if (record.CourseSeed == entry.CourseSeed && entry.TimeCompleted < record.TimeCompleted)
            {
                previousBestTime = record;
                return true;
            }
        }

        return false;
    }

    private List<CourseHistoryEntry> GetCurrentRecords()
    {
        if (PlayerPrefs.HasKey("CourseRecords"))
        {
            var serializedList = PlayerPrefs.GetString("CourseRecords");
            var historyListObject = JsonUtility.FromJson<CourseHistoryList>(serializedList);
            return historyListObject.entries;
        }
        else
        {
            return new List<CourseHistoryEntry>();
        }
    }

    private void SerializeAndSave(List<CourseHistoryEntry> currentRecords)
    {
        // Fucking unity doesn't support top-level array serialization?
        // What the fuck is this shit?
        var list = new CourseHistoryList();
        list.entries = currentRecords;
        var serializedList = JsonUtility.ToJson(list);
        PlayerPrefs.SetString("CourseRecords", serializedList);
    }

    void Start () { }
	void Update () { }

    [System.Serializable]
    class CourseHistoryEntry : object
    {
        [SerializeField]
        public int CourseSeed;
        [SerializeField]
        public float TimeCompleted;
        [SerializeField]
        public bool IsFavorited;
    }

    [Serializable]
    class CourseHistoryList
    {
        public List<CourseHistoryEntry> entries;
    }
}
