using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConcPerfectNetworkManager : MonoBehaviour {
	void Start () {
        GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StartHost();
    }

    public void QuitServer()
    {
        GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManager>().StopServer();
    }
}
