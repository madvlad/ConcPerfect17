using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BeaconMarker : NetworkBehaviour
{
    [SyncVar]
    public string ownedByTeam = "";
    [SyncVar]
    public float currentTimer;
    [SyncVar]
    public bool guarded = false;
    [SyncVar]
    public string lastCapturer = "None";
    [SyncVar]
    public NetworkInstanceId lastCapturerNetId;

    public float CurrentTimer
    {
        get
        {
            return currentTimer;
        }

        set
        {
            currentTimer = value;
        }
    }

    public bool Guarded
    {
        get
        {
            return guarded;
        }

        set
        {
            guarded = value;
        }
    }

    public string LastCapturer
    {
        get
        {
            return lastCapturer;
        }

        set
        {
            lastCapturer = value;
        }
    }

    public NetworkInstanceId LastCapturerNetId
    {
        get
        {
            return lastCapturerNetId;
        }

        set
        {
            lastCapturerNetId = value;
        }
    }

    public string OwnedByTeam
    {
        get
        {
            return ownedByTeam;
        }

        set
        {
            ownedByTeam = value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ClientRpc]
    public void RpcUpdateBeaconMaterial()
    {
        Debug.Log("Called RPCUPdateBeaconMat");
        GetComponentInChildren<BeaconScript>().ChangeBeaconMaterial();
    }
}
