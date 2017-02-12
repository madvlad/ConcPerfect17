using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseHistoryManager : MonoBehaviour {

    private CourseHistoryEntry previousBestTime;
    
    public void StoreNewRecord(string CourseSeed, float TimeCompleted, bool IsFavorited)
    {
        var entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, IsFavorited = IsFavorited };
        var currentRecords = GetCurrentRecords();
        bool listChanged = false;

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
            if (String.Equals(record.CourseSeed, entry.CourseSeed))
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
            if (String.Equals(record.CourseSeed, entry.CourseSeed) && entry.TimeCompleted < record.TimeCompleted)
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
            return JsonUtility.FromJson<List<CourseHistoryEntry>>(serializedList);
        }
        else
        {
            return new List<CourseHistoryEntry>();
        }
    }

    private void SerializeAndSave(List<CourseHistoryEntry> currentRecords)
    {
        string serializedList = JsonUtility.ToJson(currentRecords);
        PlayerPrefs.SetString("CourseRecords", serializedList);
    }

    void Start () { }
	void Update () { }

    [Serializable]
    private class CourseHistoryEntry
    {
        public string CourseSeed;
        public float TimeCompleted;
        public bool IsFavorited;
    }
}
