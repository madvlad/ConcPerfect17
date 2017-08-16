using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!SteamManager.Initialized)
            return;

        bool bSuccess = SteamUserStats.RequestCurrentStats();

        
	}
}
