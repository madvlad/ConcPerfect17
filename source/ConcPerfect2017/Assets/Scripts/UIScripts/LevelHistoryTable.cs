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
            var entryInstance = Instantiate(EntryPrefab);
            var entryManager = entryInstance.GetComponent<HistoryEntryManager>();
            entryManager.SetFields(idx.ToString(), entry.CourseSeed.ToString(), entry.TimeCompleted.ToString(), entry.IsFavorited);
            entryInstance.transform.parent = GridElement.transform;
            idx++;
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
