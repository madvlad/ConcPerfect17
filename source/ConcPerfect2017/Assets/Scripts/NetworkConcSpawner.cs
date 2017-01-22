using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkConcSpawner : NetworkBehaviour {
    public void SpawnConc(GameObject concToSpawn)
    {
        NetworkServer.Spawn(concToSpawn);
    }
}
