using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class PondAchievement : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SteamManager.Initialized)
            {
                SteamUserStats.SetAchievement("ACHIEVEMENT_DRENCHED");
            }
        }
    }
}