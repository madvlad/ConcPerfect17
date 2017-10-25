using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConcminationStarter : NetworkBehaviour {
    public GameObject RenderedCylinder;
    public AudioClip TriggeredSound;

    [SyncVar]
    private float Timer = 10;
    [SyncVar]
    private bool IsTriggered = false;
    [SyncVar]
    private bool GateOpened = false;

    private bool HasPlayed = false;

    void OnStart() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CmdStartTimeCountdown(gameObject.GetComponent<NetworkIdentity>(), other.gameObject.GetComponent<NetworkIdentity>());
        }
    }

    void FixedUpdate ()
    {
        if (!HasPlayed && IsTriggered)
        {
            GetComponent<AudioSource>().PlayOneShot(TriggeredSound, ApplicationManager.sfxVolume);
            HasPlayed = true;
        }
    }

    public void ResetStarter()
    {
        IsTriggered = false;
        HasPlayed = false;
        RenderedCylinder.GetComponent<MeshRenderer>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }
    
    [Command]
    void CmdStartTimeCountdown(NetworkIdentity grabId, NetworkIdentity player)
    {
        grabId.AssignClientAuthority(player.connectionToClient);
        IsTriggered = true;
        RenderedCylinder.GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }
}
