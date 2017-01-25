using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConcPerfectNetworkManager : MonoBehaviour {
	void Start () {
        if (ApplicationManager.IsSingleplayer)
        {
            GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartHost();
        }
        else
        {
            GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().networkPort = 7777;
            GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().networkAddress = ApplicationManager.NetworkAddress;
            GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartClient();
        }
    }

    public void QuitServer()
    {
        GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StopServer();
    }
}
