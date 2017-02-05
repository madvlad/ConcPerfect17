using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class ConcPerfectNetworkManager : MonoBehaviour {

	void Start () {
        if (ApplicationManager.IsSingleplayer)
        {
            if (ApplicationManager.IsLAN) {
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartHost();
            } else {
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartMatchMaker();
                NetworkMatch matchMaker = GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().matchMaker;
                matchMaker.CreateMatch(ApplicationManager.ServerName, ApplicationManager.MaxNumberOfPlayers, ApplicationManager.AdvertiseServer, ApplicationManager.ServerPassword, "", "", 0, 0, OnInternetMatchCreate);
            }
        }
        else
        {
            if (ApplicationManager.IsLAN) {
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().networkPort = 7777;
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().networkAddress = ApplicationManager.NetworkAddress;
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartClient();
            } else {
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartMatchMaker();
                GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().matchMaker.ListMatches(0, 10, ApplicationManager.ServerName, true, 0, 0, OnInternetMatchList);
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
                 GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
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
            //TODO: Return to match list menu
        }
    }
}
