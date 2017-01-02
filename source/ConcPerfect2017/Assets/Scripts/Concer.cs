﻿using UnityEngine;
using System.Collections;

public class Concer : MonoBehaviour
{

    public GameObject concPrefab;
    public int ConcCount = 3;
    private bool primed = false;
    private float timer = 0.0f;
    private GameObject concInstance;
    private GameObject playerCamera;

    void Start()
    {
        playerCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Conc") && ConcCount > 0)
        {
            if (timer <= 0)
            {
                ConcCount--;
                timer = 0.45f;
                primed = true;
                concInstance = Instantiate(concPrefab, playerCamera.transform.position, playerCamera.transform.rotation) as GameObject;
                if (!concInstance.GetComponent<Rigidbody>()) { concInstance.AddComponent<Rigidbody>(); }
                concInstance.GetComponent<Rigidbody>().useGravity = false;
                concInstance.GetComponent<BoxCollider>().enabled = false;
                concInstance.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        if (primed && !Input.GetButton("Conc"))
        {
            if (concPrefab && !concInstance.GetComponent<Conc>().exploded)
            {
                concInstance.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
                concInstance.GetComponent<BoxCollider>().enabled = true;
                concInstance.GetComponent<MeshRenderer>().enabled = true;
                concInstance.GetComponent<Rigidbody>().useGravity = true;
                concInstance.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * 8, ForceMode.Impulse);
                primed = false;
            }
        }
        else if (primed)
        {
            concInstance.transform.position = playerCamera.transform.position;
        }
    }
}