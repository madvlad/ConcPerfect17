using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStarter : MonoBehaviour {

    public GameObject Trigger;
    public GameObject TriggerPlatform;
    public GameObject Barrier;
    public AudioClip GoSound;

    private int Timer = 30;

    void OnTriggerEnter(Collider other)
    {
        Trigger.GetComponent<Renderer>().enabled = false;
        TriggerPlatform.GetComponent<Renderer>().enabled = false;

        Debug.Log("Gate will open in " + Timer + " seconds");
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("LongCountdown", 5.0f);
        }
    }

    void LongCountdown()
    {
        Timer -= 5;
        Debug.Log("Gate will open in " + Timer + " seconds");

        if (Timer > 10)
        {
            Invoke("LongCountdown", 5.0f);
        }
        else
        {
            Invoke("ShortCountdown", 1.0f);
        }
    }

    void ShortCountdown()
    {
        Timer -= 1;
        Debug.Log("Gate will open in " + Timer + " seconds");

        if (Timer > 0)
        {
            Invoke("ShortCountdown", 1.0f);
        }
        else
        {
            ReleaseGate();
            Invoke("ShutGate", 30.0f);
        }
    }

    private void ReleaseGate()
    {
        AudioSource.PlayClipAtPoint(GoSound, gameObject.transform.position);
        Barrier.SetActive(false);
        Debug.Log("Gate open");
    }

    void ShutGate()
    {
        Barrier.SetActive(true);
        Debug.Log("Gate closed");
    }
}
