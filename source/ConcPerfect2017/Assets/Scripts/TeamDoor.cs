using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamDoor : NetworkBehaviour {
    TeamManager teamManager;
    public string TeamName;
    public Material TeamSkin;

	// Use this for initialization
	void Start () {
        if (!isServer)
            return;

        teamManager = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerInfo pInfo = new PlayerInfo();
            pInfo.PlayerId = other.GetComponent<NetworkIdentity>().netId;
            pInfo.Nickname = ApplicationManager.Nickname;
            GetLocalPlayerObject().GetComponent<LocalPlayerStats>().CmdAddPlayerToTeam(TeamName, pInfo);
            other.gameObject.GetComponent<Concer>().CurrentTeam = TeamName;
            other.gameObject.GetComponent<FirstPersonDrifter>().playerModelRenderer.GetComponent<SkinnedMeshRenderer>().material = TeamSkin;
        }
    }

    private GameObject GetLocalPlayerObject() {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject playerObject = null;
        foreach (GameObject obj in playerObjects) {
            if (obj.GetComponent<NetworkIdentity>().isLocalPlayer) {
                playerObject = obj;
            }
        }

        return playerObject;
    }
}
