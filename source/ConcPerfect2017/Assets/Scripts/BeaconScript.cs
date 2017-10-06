using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BeaconScript : NetworkBehaviour {
    public List<Material> TeamMaterials;
    public string BeaconName = "Conc Beacon";
    public float SafeTimerSeconds = 15.0f;

    [SyncVar]
    public string OwnedByTeam = "";
    [SyncVar]
    private float currentTimer;
    [SyncVar]
    private bool guarded = false;
    [SyncVar]
    public string LastCapturer = "None";
    [SyncVar]
    public NetworkInstanceId LastCapturerNetId;

    // Use this for initialization
    void Start () {
        currentTimer = 0.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (currentTimer > 0.0f)
        {
            currentTimer -= Time.deltaTime;
        }
	}

    private void OnTriggerExit(Collider other)
    {
        var colliderTeam = other.gameObject.GetComponent<Concer>().CurrentTeam;

        if (colliderTeam.Equals(OwnedByTeam))
            guarded = false;
    }

    void OnTriggerStay(Collider collider)
    {
        var colliderTeam = collider.gameObject.GetComponent<Concer>().CurrentTeam;
        
        if (colliderTeam.Equals(OwnedByTeam))
            guarded = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            var colliderTeam = collider.gameObject.GetComponent<Concer>().CurrentTeam;

            if (currentTimer <= 0.0f && !guarded)
            {
                if (!colliderTeam.Equals(OwnedByTeam))
                {
                    OwnedByTeam = collider.gameObject.GetComponent<Concer>().CurrentTeam;
                    currentTimer = SafeTimerSeconds;
                    Debug.Log("Now owned by team " + OwnedByTeam);

                    switch(OwnedByTeam)
                    {
                        case "Red Rangers":
                            gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[1];
                            break;
                        case "Green Gorillas":
                            gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[2];
                            break;
                        case "Blue Bandits":
                            gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[3];
                            break;
                        case "Yellow Yahoos":
                            gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[4];
                            break;
                        default:
                            gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[0];
                            break;

                    }

                    LastCapturer = ApplicationManager.Nickname;
                    GameObject localPlayer = ApplicationManager.GetLocalPlayerObject();
                    LastCapturerNetId = localPlayer.GetComponent<NetworkIdentity>().netId;
                    localPlayer.GetComponent<LocalPlayerStats>().UpdateCapturedBeacons();

                    collider.gameObject.GetComponent<MultiplayerChatScript>().SendBeaconCaptureMessage(BeaconName);
                    gameObject.GetComponent<AudioSource>().volume = ApplicationManager.sfxVolume;
                    gameObject.GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}
