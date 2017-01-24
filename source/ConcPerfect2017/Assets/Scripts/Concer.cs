using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Concer : NetworkBehaviour
{
    public GameObject concPrefab;
    public int ConcPushForce = 8;
    public int ConcCount = 3;
    public int MaxConcCount = 3;

    private GameObject concCountHUDElement;
    private GameObject concPrimedHUDElement;
    private bool primed = false;
    private float timer = 0.0f;
    private GameObject concInstance;
    private GameObject playerCamera;

    public override void OnStartLocalPlayer()
    {
        playerCamera = Camera.main.gameObject;
        concCountHUDElement = GameObject.FindGameObjectWithTag("ConcCounter");
        concPrimedHUDElement = GameObject.FindGameObjectWithTag("PrimedNotification");
        concPrimedHUDElement.SetActive(false);
    }

    public void SetConcCount(int newConcCount)
    {
        concCountHUDElement.GetComponent<Text>().text = "Concs: " + newConcCount;
        ConcCount = newConcCount;
    }
    
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (concInstance == null || concInstance.GetComponent<Conc>().exploded)
        {
            primed = false;
            concPrimedHUDElement.SetActive(false);
        }

        if (Input.GetButtonDown("Conc") && ConcCount > 0 && !primed)
        {
            if (timer <= 0)
            {
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