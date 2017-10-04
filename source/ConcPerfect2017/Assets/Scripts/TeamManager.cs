using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TeamManager : NetworkBehaviour {
    private string[] TEAM_NAMES = { "Team1", "Team2", "Team3", "Team4" };
    private Color[] TEAM_COLORS = { Color.red, Color.blue, Color.green, Color.yellow };
    
    private Dictionary<string, Team> teams;

    private int numberOfTeams;

    public int NumberOfTeams {
        get {
            return numberOfTeams;
        }

        set {
            numberOfTeams = value;
        }
    }

    void Start() {
        // TODO - Initialize values based on host params
        teams = new Dictionary<string, Team>();
        numberOfTeams = 4;
        SetUpTeams(numberOfTeams);
    }

    public void SetUpTeams(int numberOfTeams) {
        if (!isServer)
            return;

        this.numberOfTeams = numberOfTeams;
        for (int i=0; i<numberOfTeams; i++) {
            teams.Add(TEAM_NAMES[i], new Team(TEAM_COLORS[i], TEAM_NAMES[i]));
        }
    }

    public void AddPlayerToTeam(string teamName, PlayerInfo pInfo) {
        teams[teamName].addPlayerToTeam(pInfo);
        // TODO - Change the player skinz
    }
}