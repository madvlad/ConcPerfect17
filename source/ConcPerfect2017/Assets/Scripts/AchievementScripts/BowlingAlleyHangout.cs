using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class BowlingAlleyHangout : MonoBehaviour
{

    float timeSpentHere = 0.0f;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timeSpentHere += Time.deltaTime;
            if (timeSpentHere >= 60)
            {
                if (SteamManager.Initialized)
                {
                    SteamUserStats.SetAchievement("ACHIEVEMENT_THRASHER");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timeSpentHere = 0.0f;
        }
    }
}