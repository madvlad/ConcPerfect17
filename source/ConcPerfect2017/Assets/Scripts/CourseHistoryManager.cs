using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseHistoryManager : MonoBehaviour {

    private CourseHistoryEntry previousBestTime;

    public float GetCurrentCourseRecord(int CourseSeed)
    {
        var currentRecords = GetSavedRecords("CourseRecords");

        foreach (var record in currentRecords)
        {
            if (record.CourseSeed == CourseSeed)
            {
                return record.TimeCompleted;
            }
        }
        return float.PositiveInfinity;
    }

    public void FavoriteCourse(int CourseSeed, bool IsFavorited)
    {
        var TimeCompleted = GetCurrentCourseRecord(CourseSeed);
        var entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, IsFavorited = IsFavorited };
        StoreNewRecord(CourseSeed, TimeCompleted, IsFavorited);
    }

    public bool IsCourseFavorited(int CourseSeed)
    {
        var currentRecords = GetSavedRecords("CourseRecords");

        foreach (var record in currentRecords)
        {
            if (record.CourseSeed == CourseSeed)
            {
                return record.IsFavorited;
            }
        }
        return false;
    }

    public void StoreNewRecord(int CourseSeed, float TimeCompleted, bool IsFavorited)
    {
        var entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, IsFavorited = IsFavorited };
        var currentRecords = GetSavedRecords("CourseRecords");
        
        if (IsFirstRecord(currentRecords, entry))
        {
            currentRecords.Add(entry);
            SerializeAndSaveRecords("CourseRecords", currentRecords);
        }
        else if (IsBestTime(currentRecords, entry))
        {
            currentRecords.Remove(previousBestTime);
            currentRecords.Add(entry);
            SerializeAndSaveRecords("CourseRecords", currentRecords);
        }
    }

    public void AddRecentlyPlayed(int CourseSeed, float TimeCompleted)
    {
        var recentPlayed = GetSavedRecords("RecentPlayed");
        recentPlayed.Insert(0, new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted });
        SerializeAndSaveRecords("RecentPlayed", recentPlayed);
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
            if (record.CourseSeed == entry.CourseSeed && entry.TimeCompleted <= record.TimeCompleted)
            {
                previousBestTime = record;
                return true;
            }
        }

        return false;
    }

    public List<CourseHistoryEntry> GetSavedRecords(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            var serializedList = PlayerPrefs.GetString(key);
            var historyListObject = JsonUtility.FromJson<CourseHistoryList>(serializedList);
            return historyListObject.entries;
        }
        else
        {
            return new List<CourseHistoryEntry>();
        }
    }

    private void SerializeAndSaveRecords(string key, List<CourseHistoryEntry> currentRecords)
    {
        // Fucking unity doesn't support top-level array serialization?
        // What the fuck is this shit?
        var list = new CourseHistoryList();
        list.entries = currentRecords;
        var serializedList = JsonUtility.ToJson(list);
        PlayerPrefs.SetString(key, serializedList);
    }

    void Start () { }
	void Update () { }

    [Serializable]
    public class CourseHistoryEntry : object
    {
        [SerializeField]
        public int CourseSeed;
        [SerializeField]
        public float TimeCompleted;
        [SerializeField]
        public bool IsFavorited;
    }

    [Serializable]
    public class CourseHistoryList
    {
        public List<CourseHistoryEntry> entries;
    }
}
