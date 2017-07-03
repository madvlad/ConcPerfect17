using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseHistoryManager : MonoBehaviour {

    private CourseHistoryEntry previousBestTime;

    public float GetCurrentCourseRecordBySeed(int CourseSeed, int DifficultyLevel)
    {
        var currentRecords = GetSavedRecords("CourseRecords");

        foreach (var record in currentRecords)
        {
            if (record.CourseSeed == CourseSeed && record.DifficultyLevel == DifficultyLevel)
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

    public void FavoriteCourse(int CourseSeed, int DifficultyLevel, bool IsFavorited)
    {
        var TimeCompleted = GetCurrentCourseRecordBySeed(CourseSeed, DifficultyLevel);
        StoreNewRecord(CourseSeed, TimeCompleted, IsFavorited, DifficultyLevel);
    }

    public bool IsCourseFavorited(int CourseSeed, int DifficultyLevel)
    {
        var currentRecords = GetSavedRecords("CourseRecords");

        foreach (var record in currentRecords)
        {
            if (record.CourseSeed == CourseSeed && record.DifficultyLevel == DifficultyLevel)
            {
                return record.IsFavorited;
            }
        }
        return false;
    }

    public bool StoreNewRecord(int CourseSeed, float TimeCompleted, bool IsFavorited, int DifficultyLevel)
    {
        CourseHistoryEntry entry;

        if (CourseSeed == 0)
        {
            entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, IsFavorited = IsFavorited, CourseName = GetCourseName(ApplicationManager.currentLevel), DifficultyLevel = 0 };
        }
        else
        {
            entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, IsFavorited = IsFavorited, DifficultyLevel = DifficultyLevel };
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

    public void AddRecentlyPlayed(int CourseSeed, float TimeCompleted, int DifficultyLevel)
    {
        var recentPlayed = GetSavedRecords("RecentPlayed");
        CourseHistoryEntry entry;

        if (CourseSeed == 0)
        {
            entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, CourseName = GetCourseName(ApplicationManager.currentLevel), DifficultyLevel = DifficultyLevel };
        }
        else
        {
            entry = new CourseHistoryEntry { CourseSeed = CourseSeed, TimeCompleted = TimeCompleted, DifficultyLevel = DifficultyLevel };
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
                if (record.CourseSeed == entry.CourseSeed && record.DifficultyLevel == entry.DifficultyLevel)
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
                if (record.CourseSeed == entry.CourseSeed && entry.TimeCompleted <= record.TimeCompleted && entry.DifficultyLevel == record.DifficultyLevel)
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
            case 6:
                return "Nanob1te's Mainframe";
            default:
                return "Tutor's Gorge";
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
        public int DifficultyLevel;
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
