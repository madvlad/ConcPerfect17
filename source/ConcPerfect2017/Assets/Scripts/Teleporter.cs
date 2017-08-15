using UnityEngine;
using System.Collections;
using Steamworks;

public class Teleporter : MonoBehaviour
{
    public GameObject destination;
    public bool StopMomentum = true;

    public bool IsSecret = false;
    public AudioClip SecretMusic;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SteamManager.Initialized)
            {
                ApplicationManager.respawnCount++;
                if (ApplicationManager.respawnCount >= 50)
                {
                    SteamUserStats.SetAchievement("ACHIEVEMENT_10_JUMPS");
                }
            }

            other.transform.position = destination.transform.position;
            other.transform.rotation = Quaternion.Euler(destination.transform.rotation.x, destination.transform.rotation.y, destination.transform.rotation.z);

            if (StopMomentum)
            {
                other.gameObject.GetComponent<ImpactReceiver>().ZeroImpact();
            }

            if (IsSecret)
            {
                GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().clip = SecretMusic;
                GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();
            }
        }
    }
}