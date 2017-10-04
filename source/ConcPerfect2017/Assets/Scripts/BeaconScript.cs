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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // get player team
            // set owned by team to the current player's team
            if (currentTimer <= 0.0f)
            {
                if (collider.gameObject.GetComponent<Concer>().CurrentTeam != OwnedByTeam)
                {
                    OwnedByTeam = collider.gameObject.GetComponent<Concer>().CurrentTeam;
                    currentTimer = SafeTimerSeconds;
                    Debug.Log("Now owned by team " + OwnedByTeam);

                    switch(OwnedByTeam)
                    {
                        case "Red Rangers":
                            gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[1];
                            break;
                        default:
                            gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[0];
                            break;

                    }
                }
            }
        }
    }
}
