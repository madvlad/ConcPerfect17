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
    public float CountdownDuration;

    [SyncVar]
    private float Timer;
    [SyncVar]
    private bool IsTriggered = false;
    [SyncVar]
    private bool GateOpened = false;

    void OnStart()
    {
        Timer = CountdownDuration;
        Trigger.GetComponent<Renderer>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        TriggerPlatform.GetComponent<Renderer>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Timer = CountdownDuration;
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
            GetComponent<CapsuleCollider>().enabled = false;

            if (Timer > 0)
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
        GetComponent<AudioSource>().PlayOneShot(GoSound, ApplicationManager.sfxVolume);
        Barrier.SetActive(false);
        if (ApplicationManager.GameType == GameTypes.ConcminationGameType) {
            GetComponent<SetTimerOnTrigger>().StartTimer();
        }
    }

    void ShutGate()
    {
        if (ApplicationManager.GameType == GameTypes.ConcminationGameType)
        {
            GetComponent<SetTimerOnTrigger>().StopTimer();
        }
        else
        {
            Barrier.SetActive(true);
        }

        GateOpened = false;
    }

    public void RestartGate()
    {
        if (Barrier != null)
        {
            Barrier.SetActive(true);
        }
        Timer = CountdownDuration;
        Trigger.SetActive(true);
        TriggerPlatform.SetActive(true);
        if (ApplicationManager.GameType == GameTypes.ConcminationGameType)
        {
            GetComponent<SetTimerOnTrigger>().StopTimer();
        }
    }

    [ClientRpc]
    public void RpcRestartGate()
    {
        RestartGate();
    }
}
