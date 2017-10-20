using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BeaconManager : MonoBehaviour {
    private List<Team> winners;

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

    public Dictionary<Team, int> GetTeamBeaconCount() {
        winners = new List<Team>();
        TeamManager tm = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>();
        Dictionary<Team, int> teamScores = new Dictionary<Team, int>();
        foreach (Team t in tm.GetTeams().Values) {
            teamScores[t] = 0;
        }
        foreach (GameObject beacon in GameObject.FindGameObjectsWithTag("Beacon")) {
            BeaconScript bc = beacon.GetComponent<BeaconScript>();
            if (bc.LastCapturer != "None" && bc.OwnedByTeam != null) {
                Team BeaconTeam = tm.GetTeamByName(beacon.GetComponent<BeaconScript>().OwnedByTeam);
                teamScores[BeaconTeam] += 1;
            }
        }

        int highscore = -1;
        foreach (KeyValuePair<Team, int> kvp in teamScores) {
            if (kvp.Value > highscore) {
                highscore = kvp.Value;
                winners.Clear();
                winners.Add(kvp.Key);
            } else if (kvp.Value == highscore) {
                winners.Add(kvp.Key);
            }
        }
        return teamScores;
    }

    public List<Team> GetWinners() {
        return winners;
    }
}
