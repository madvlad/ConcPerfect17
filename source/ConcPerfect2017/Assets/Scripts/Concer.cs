using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

public class Concer : NetworkBehaviour
{
    public GameObject concPrefab;
    public int ConcPushForce = 8;
    public int ConcCount = 3;
    public int MaxConcCount = 3;
    public string CurrentTeam = "Red Rangers";
    private GameObject[] ConcTimers;

    private GameObject concCountHUDElement;
    private GameObject concPrimedHUDElement;
    private bool primed = false;
    private float timer = 0.0f;
    private int currentConc = 2;
    private GameObject concInstance;
    private GameObject playerCamera;
    private bool NotFaded = false;

    public override void OnStartLocalPlayer()
    {
        playerCamera = Camera.main.gameObject;
        concCountHUDElement = GameObject.FindGameObjectWithTag("ConcCounter");
        concPrimedHUDElement = GameObject.FindGameObjectWithTag("PrimedNotification");
        ConcTimers = GameObject.FindGameObjectsWithTag("VisualTimer");
        concPrimedHUDElement.SetActive(false);
    }

    public void SetConcCount(int newConcCount)
    {
        concCountHUDElement.GetComponent<Text>().text = newConcCount +"/3";
        ConcCount = newConcCount;
    }
    
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (ConcTimersNotPlaying())
        {
            currentConc = 2;
            if (NotFaded)
            {
                NotFaded = false;
                GameObject.FindGameObjectWithTag("VisualTimerPanel").GetComponent<Animation>().Play();
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (concInstance == null || concInstance.GetComponent<Conc>().exploded)
        {
            primed = false;
            concPrimedHUDElement.SetActive(false);
        }

        if (Input.GetButtonDown("Conc") && ConcCount > 0 && !primed && !GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>().IsGamePause() && !GetLocalPlayerObject().GetComponent<FirstPersonDrifter>().IsEscaped() && !GetLocalPlayerObject().GetComponent<FirstPersonDrifter>().IsFrozen())
        {
            if (timer <= 0)
            {
                StartVisualTimer();
                SetConcCount(ConcCount - 1);
                timer = 0.45f;
                primed = true;
                concPrimedHUDElement.SetActive(true);
                concInstance = Instantiate(concPrefab, playerCamera.transform.position, playerCamera.transform.rotation);
                concInstance.GetComponent<Conc>().SetOwner(GetLocalPlayerObject());
                if (!concInstance.GetComponent<Rigidbody>()) { concInstance.AddComponent<Rigidbody>(); }
                concInstance.GetComponent<Rigidbody>().useGravity = false;
                concInstance.GetComponent<BoxCollider>().enabled = false;
                concInstance.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        if (primed && !Input.GetButton("Conc"))
        {
            if (concPrefab && concInstance != null && concInstance.GetComponent<Conc>() != null && !concInstance.GetComponent<Conc>().exploded)
            {
                concInstance.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
                concInstance.GetComponent<BoxCollider>().enabled = true;
                concInstance.GetComponent<MeshRenderer>().enabled = true;
                concInstance.GetComponent<Rigidbody>().useGravity = true;
                concInstance.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * ConcPushForce, ForceMode.Impulse);
                primed = false;
                concPrimedHUDElement.SetActive(false);
            }
        }
        else if (primed)
        {
            concInstance.transform.position = playerCamera.transform.position;
        }
    }

    private bool ConcTimersNotPlaying()
    {
        foreach (var timer in ConcTimers)
        {
            if (timer.GetComponent<Animation>().isPlaying)
            {
                return false;
            }
        }
        return true;
    }

    private void StartVisualTimer()
    {
        NotFaded = true;
        GameObject.FindGameObjectWithTag("VisualTimerPanel").GetComponent<Animation>().Play("FadeTimerPauseAnimation");
        if (ConcTimers[currentConc].GetComponent<Animation>().isPlaying)
        {
            ConcTimers[currentConc].GetComponent<Animation>().Rewind();
        }
        else
        {
            ConcTimers[currentConc].GetComponent<Animation>().Play();
        }

        if (currentConc == 0)
        {
            currentConc = 2;
        }
        else
        {
            currentConc--;
        }
    }

    private GameObject GetLocalPlayerObject()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject playerObject = null;
        foreach (GameObject obj in playerObjects)
        {
            if (obj.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerObject = obj;
            }
        }

        return playerObject;
    }
}