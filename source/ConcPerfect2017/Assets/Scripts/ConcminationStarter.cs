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

    void OnStart()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CmdStartTimeCountdown(gameObject.GetComponent<NetworkIdentity>(), other.gameObject.GetComponent<NetworkIdentity>());
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
		
	}
    
    [Command]
    void CmdStartTimeCountdown(NetworkIdentity grabId, NetworkIdentity player)
    {
        grabId.AssignClientAuthority(player.connectionToClient);
        IsTriggered = true;
        RenderedCylinder.GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<AudioSource>().PlayOneShot(TriggeredSound, ApplicationManager.sfxVolume);
    }
}
