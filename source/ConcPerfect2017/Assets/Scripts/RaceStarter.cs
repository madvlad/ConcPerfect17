using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RaceStarter : NetworkBehaviour {

    public GameObject Trigger;
    public GameObject TriggerPlatform;
    public GameObject Barrier;
    public AudioClip GoSound;

    [SyncVar]
    private float Timer = 10;
    [SyncVar]
    private bool IsTriggered = false;
    [SyncVar]
    private bool GateOpened = false;

    void OnStart()
    {
            Trigger.GetComponent<Renderer>().enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
            TriggerPlatform.GetComponent<Renderer>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        CmdStartTimeYaNobHead();
    }

    [Command]
    void CmdStartTimeYaNobHead()
    {
        IsTriggered = true;
    }

    void FixedUpdate()
    {
        if (IsTriggered && !GateOpened)
        {
            var GatePanel = GameObject.FindGameObjectWithTag("GatePanel").GetComponent<Text>();
            Trigger.SetActive(false);
            TriggerPlatform.SetActive(false);

            if (Timer > 10)
            {
                if (Math.Floor(Timer) % 5 == 0)
                {
                    GatePanel.text = ("Gate opens in " + Math.Floor(Timer) + " seconds");
                }
                Timer -= Time.deltaTime;
            }
            else if (Timer > 0)
            {
                GatePanel.text = ("Gate opens in " + Math.Floor(Timer) + " seconds");
                Timer -= Time.deltaTime;
            }
            else
            {
                GatePanel.text = "";
                ReleaseGate();
            }
        }
        else
        {
            if (Timer < 0)
            {
                Invoke("ShutGate", 10.0f);
            }
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
        GateOpened = true;
        IsTriggered = false;
        GetComponent<AudioSource>().PlayOneShot(GoSound);
        Barrier.SetActive(false);
        Debug.Log("Gate open");
    }

    void ShutGate()
    {
        Barrier.SetActive(true);
        Debug.Log("Gate closed");
    }
}
