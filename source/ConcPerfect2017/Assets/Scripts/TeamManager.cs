using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TeamManager : NetworkBehaviour {
    private Dictionary<string, Color> TEAM_VALUES = new Dictionary<string, Color>() {
        {"Red Rangers", Color.red },
        {"Blue Bandits", Color.blue },
        {"Green Gorillas", Color.green },
        {"Yellow Yahoos", Color.yellow }
    };

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
    }

    public void AddPlayerToTeam(string teamName, PlayerInfo pInfo, int skinNumber) {
        if (!teams.ContainsKey(teamName)) {
            teams.Add(teamName, new Team(TEAM_VALUES[teamName], teamName));
            this.numberOfTeams = teams.Count;
        }
        teams[teamName].addPlayerToTeam(pInfo);
        pInfo.CurrentTeam = teamName;
        GameServerManager gSM = GameObject.FindGameObjectWithTag("GameServerManager").GetComponent<GameServerManager>();
        gSM.UpdatePlayerTeam(teamName, pInfo, skinNumber);
    }

    public Team GetTeamByName(String name) {
        if (!String.IsNullOrEmpty(name))
        {
            return teams[name];
        }
        else
        {
            return null;
        }
    }


    public Dictionary<string, Team> GetTeams() {
        return teams;
    }
}