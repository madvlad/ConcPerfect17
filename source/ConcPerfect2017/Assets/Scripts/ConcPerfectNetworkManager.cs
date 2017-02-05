using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class ConcPerfectNetworkManager : MonoBehaviour {
    public GameObject MatchMakerLobbyUIElement;
    public GameObject ServerButtonPrefab;

    void Start () {
      
    }

    public void StartNetworkManager() {
        if (ApplicationManager.IsSingleplayer) {
            if (ApplicationManager.IsLAN) {
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartHost();
            } else {
                if (GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().matchMaker == null)
                    GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartMatchMaker();
                NetworkMatch matchMaker = GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().matchMaker;
                matchMaker.CreateMatch(ApplicationManager.ServerName, ApplicationManager.MaxNumberOfPlayers, ApplicationManager.AdvertiseServer, ApplicationManager.ServerPassword, "", "", 0, 0, OnInternetMatchCreate);
            }
        } else {
            if (ApplicationManager.IsLAN) {
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().networkPort = 7777;
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().networkAddress = ApplicationManager.NetworkAddress;
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartClient();
            } else {
                if (GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().matchMaker == null)
                    GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartMatchMaker();
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().matchMaker.ListMatches(0, 10, "", true, 0, 0, OnInternetMatchList);
            }
        }
    }

    public void QuitServer()
    {
        GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StopServer();
        GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StopMatchMaker();
    }

    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
        if (success) {
            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 7777);
            GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartHost(hostInfo);
        } else {
            //TODO: Return to host menu
            Debug.Log("Failed to create match" + extendedInfo);
        }
    }

    private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
        if (success) {
            if (matches.Count != 0) {
                foreach (MatchInfoSnapshot snapshot in matches) {
                    GameObject newServerButton = Instantiate(ServerButtonPrefab);
                    newServerButton.GetComponent<ServerButton>().SetMatchSnapshot(snapshot);
                    newServerButton.transform.parent = MatchMakerLobbyUIElement.transform;
                }
            } else {
                //TODO: Return to match list menu
            }
        } else {
            //TODO: Return to match list menu
        }
    }

    private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo) {
        if (success) {
            MatchInfo hostInfo = matchInfo;
            GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartClient(hostInfo);
        } else {
            foreach (GameObject server in GameObject.FindGameObjectsWithTag("ServerButtons")) {
                Destroy(server);
            }
            StartNetworkManager();
        }
    }


    public void OnSelectMatchmakingServer(MatchInfoSnapshot snapshot) {
        GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().matchMaker.JoinMatch(snapshot.networkId, "", "", "", 0, 0, OnJoinInternetMatch);
    }
}
