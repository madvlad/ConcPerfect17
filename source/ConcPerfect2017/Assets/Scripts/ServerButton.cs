using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class ServerButton : MonoBehaviour {
    private string serverName;
    private MatchInfoSnapshot snapshot; 

    public GameObject ConcPerfectNetworkManager;

    public void SetServerName(string servername) {
        this.serverName = servername;
        gameObject.GetComponentInChildren<Text>().text = serverName;
    }

    public void SetMatchSnapshot(MatchInfoSnapshot snapshot) {
        SetServerName(snapshot.name);
        this.snapshot = snapshot;
    }

    public string GetServerName() {
        return serverName;
    }

    public void OnClick() {
        GameObject.FindGameObjectWithTag("LoadingCanvas").GetComponent<Canvas>().enabled = true;
        foreach (GameObject server in GameObject.FindGameObjectsWithTag("ServerButtons")) {
            server.GetComponent<Button>().interactable = false;
        }
        GameObject.FindGameObjectWithTag("NetworkIssuer").GetComponent<ConcPerfectNetworkManager>().OnSelectMatchmakingServer(snapshot);
    }
}
