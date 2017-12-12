using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BeaconManager : NetworkBehaviour
{
    public List<GameObject> BeaconMarkers;

    private List<GameObject> beacons;
    private List<Team> winners;

    // Use this for initialization
    void Start()
    {
        if (ApplicationManager.GameType == GameTypes.ConcminationGameType)
        {
            var realBeaconMarkers = new List<GameObject>();
            if (ApplicationManager.currentLevel == 18)
            {
                realBeaconMarkers = BeaconMarkers.GetRange(9, 9);
            }
            else
            {
                realBeaconMarkers = BeaconMarkers.GetRange(0, 9);
            }

            beacons = new List<GameObject>();
            foreach (GameObject bM in realBeaconMarkers)
            {
                GameObject bMPrefab = Instantiate(bM);
                bMPrefab.transform.position = new Vector3(bMPrefab.transform.position.x + 67.81221f, bMPrefab.transform.position.y - 1.518f, bMPrefab.transform.position.z - 37.85f);
                NetworkServer.Spawn(bMPrefab);
                beacons.Add(bMPrefab.transform.GetChild(0).gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckForConcmination()
    {
        foreach (KeyValuePair<Team, int> kvp in GetTeamBeaconCount())
        {
            if (kvp.Value >= beacons.Count)
            {
                //GameObject.FindGameObjectWithTag("RaceStart").GetComponent<RaceStarter>().GetComponent<SetTimerOnTrigger>().SwitchToOn = false;
                //GameObject.FindGameObjectWithTag("RaceStart").GetComponent<RaceStarter>().GetComponent<SetTimerOnTrigger>().StopTimer();
            }
        }
    }

    public int GetUserBeaconCount(NetworkInstanceId netId)
    {
        int capturedBeacons = 0;
        foreach (GameObject beacon in beacons)
        {
            BeaconMarker parentBeaconMarker = beacon.GetComponentInParent<BeaconMarker>();
            if (beacon.GetComponentInParent<BeaconMarker>().LastCapturerNetId != null && beacon.GetComponentInParent<BeaconMarker>().LastCapturerNetId == netId)
            {
                capturedBeacons++;
            }
        }
        return capturedBeacons;
    }

    public Dictionary<Team, int> GetTeamBeaconCount()
    {
        winners = new List<Team>();
        TeamManager tm = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>();
        Dictionary<Team, int> teamScores = new Dictionary<Team, int>();
        foreach (Team t in tm.GetTeams().Values)
        {
            teamScores[t] = 0;
        }
        foreach (GameObject beacon in beacons)
        {
            BeaconMarker bc = beacon.GetComponentInParent<BeaconMarker>();
            if (bc.LastCapturer != "None" && (bc.OwnedByTeam != "" || bc.OwnedByTeam != null))
            {
                Team BeaconTeam = tm.GetTeamByName(beacon.GetComponentInParent<BeaconMarker>().OwnedByTeam);
                if (BeaconTeam != null)
                {
                    teamScores[BeaconTeam] += 1;
                }
            }
        }

        int highscore = -1;
        foreach (KeyValuePair<Team, int> kvp in teamScores)
        {
            if (kvp.Value > highscore)
            {
                highscore = kvp.Value;
                winners.Clear();
                winners.Add(kvp.Key);
            }
            else if (kvp.Value == highscore)
            {
                winners.Add(kvp.Key);
            }
        }
        return teamScores;
    }

    public List<Team> GetWinners()
    {
        return winners;
    }

    public void ResetBeacons()
    {
        foreach (GameObject beacon in beacons)
        {
            beacon.GetComponentInParent<BeaconMarker>().LastCapturer = "";
            beacon.GetComponentInParent<BeaconMarker>().LastCapturerNetId = NetworkInstanceId.Invalid;
            beacon.GetComponentInParent<BeaconMarker>().CurrentTimer = 0.0f;
            beacon.GetComponentInParent<BeaconMarker>().OwnedByTeam = "";
        }

        CheckForConcmination();
    }

    internal void UpdateBeaconCapturer(string capturingTeam, string nickname, NetworkInstanceId playerId, NetworkInstanceId beaconId, float time)
    {
        foreach (GameObject beacon in beacons)
        {
            if (beacon.GetComponentInParent<NetworkIdentity>().netId == beaconId)
            {
                beacon.GetComponentInParent<BeaconMarker>().LastCapturer = nickname;
                beacon.GetComponentInParent<BeaconMarker>().LastCapturerNetId = playerId;
                beacon.GetComponentInParent<BeaconMarker>().CurrentTimer = time;
                beacon.GetComponentInParent<BeaconMarker>().OwnedByTeam = capturingTeam;
            }
        }
        CheckForConcmination();
    }
}
