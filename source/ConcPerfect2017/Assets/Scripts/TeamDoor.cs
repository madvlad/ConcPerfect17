using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamDoor : NetworkBehaviour {
    TeamManager teamManager;
    public string TeamName;
    public Material TeamSkin;
    public int TeamSkinNumber;

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
            GetLocalPlayerObject().GetComponent<LocalPlayerStats>().CmdAddPlayerToTeam(TeamName, pInfo, TeamSkinNumber);
            other.gameObject.GetComponent<Concer>().CurrentTeam = TeamName;
            other.gameObject.GetComponent<FirstPersonDrifter>().playerModelRenderer.GetComponent<SkinnedMeshRenderer>().material = TeamSkin;
            UpdateAllPlayerSkins();
        }
    }

    private void UpdateAllPlayerSkins()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in playerObjects)
        {
            obj.GetComponent<LocalPlayerStats>().RequestPlayerTeamSkins();
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
