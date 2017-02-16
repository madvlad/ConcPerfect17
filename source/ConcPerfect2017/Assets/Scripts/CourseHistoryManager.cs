using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseHistoryManager : MonoBehaviour {

    private CourseHistoryEntry previousBestTime;

    public float GetCurrentCourseRecordBySeed(int CourseSeed)
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

    public float GetCurrentCourseRecordByLevel(int levelNumber)
    {
        var CourseName = GetCourseName(levelNumber);
        var currentRecords = GetSavedRecords("CourseRecords");

        foreach (var record in currentRecords)
        {
            if (String.Equals(record.CourseName, CourseName))
            {
                return record.TimeCompleted;
            }
        }
        return float.PositiveInfinity;
    }

    public void FavoriteCourse(int CourseSeed, bool IsFavorited)
    {
        var TimeCompleted = GetCurrentCourseRecordBySeed(CourseSeed);
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

    public bool StoreNewRecord(int CourseSeed, float TimeCompleted, bool IsFavorited)
    {
        CourseHistoryEntry entry;

        if (CourseSeed == 0)
        {
            entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, IsFavorited = IsFavorited, CourseName = GetCourseName(ApplicationManager.currentLevel) };
        }
        else
        {
            entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, IsFavorited = IsFavorited };
        }

        var currentRecords = GetSavedRecords("CourseRecords");
        
        if (IsFirstRecord(currentRecords, entry))
        {
            currentRecords.Add(entry);
            SerializeAndSaveRecords("CourseRecords", currentRecords);
            return true;
        }
        else if (IsBestTime(currentRecords, entry))
        {
            currentRecords.Remove(previousBestTime);
            currentRecords.Add(entry);
            SerializeAndSaveRecords("CourseRecords", currentRecords);
            return true;
        }
        return false;
    }

    public void AddRecentlyPlayed(int CourseSeed, float TimeCompleted)
    {
        var recentPlayed = GetSavedRecords("RecentPlayed");
        CourseHistoryEntry entry;

        if (CourseSeed == 0)
        {
            entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, CourseName = GetCourseName(ApplicationManager.currentLevel) };
        }
        else
        {
            entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted};
        }

        recentPlayed.Insert(0, entry);
        SerializeAndSaveRecords("RecentPlayed", recentPlayed);
    }

    private bool IsFirstRecord(List<CourseHistoryEntry> currentRecords, CourseHistoryEntry entry)
    {
        if (entry.CourseSeed != 0)
        {
            foreach (var record in currentRecords)
            {
                if (record.CourseSeed == entry.CourseSeed)
                {
                    return false;
                }
            }
        }
        else
        {
            foreach (var record in currentRecords)
            {
                if (String.Equals(record.CourseName, entry.CourseName))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool IsBestTime(List<CourseHistoryEntry> currentRecords, CourseHistoryEntry entry)
    {
        if (entry.CourseSeed != 0)
        {
            foreach (var record in currentRecords)
            {
                if (record.CourseSeed == entry.CourseSeed && entry.TimeCompleted <= record.TimeCompleted)
                {
                    previousBestTime = record;
                    return true;
                }
            }
        }
        else
        {
            foreach (var record in currentRecords)
            {
                if (String.Equals(record.CourseName, entry.CourseName) && entry.TimeCompleted <= record.TimeCompleted)
                {
                    previousBestTime = record;
                    return true;
                }
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

    private string GetCourseName(int number)
    {
        switch(number)
        {
            case 1:
                return "Smooth Conc'in";
            case 2:
                return "Boris's Banging Butte";
            case 3:
                return "Wesley's Wild World";
            case 4:
                return "Gerald's Grand Gallery";
            case 5:
                return "Pete's Perilous Park";
            default:
                return "Tutor's Gourge";
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
        public string CourseName;
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
