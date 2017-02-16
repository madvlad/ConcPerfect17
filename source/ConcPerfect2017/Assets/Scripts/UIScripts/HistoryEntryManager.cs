using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryEntryManager : MonoBehaviour {
    public Text IndexLabel;
    public Text SeedOrLevelName;
    public Text Time;
    public Text Favorited;
	
    public void SetFields(string idx, string seed, string time, bool favorited)
    {
        IndexLabel.text = idx;
        SeedOrLevelName.text = seed;
        Time.text = time;
        Favorited.text = favorited ? "Y" : "N";
    }
}
