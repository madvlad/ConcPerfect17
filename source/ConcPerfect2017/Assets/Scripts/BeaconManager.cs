using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BeaconManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetUserBeaconCount(NetworkInstanceId netId) {
        int capturedBeacons = 0;
        foreach (GameObject beacon in GameObject.FindGameObjectsWithTag("Beacon")) {
            if (beacon.GetComponent<BeaconScript>().LastCapturerNetId == netId) {
                capturedBeacons++;
            }
        }
        return capturedBeacons;
    }
}
