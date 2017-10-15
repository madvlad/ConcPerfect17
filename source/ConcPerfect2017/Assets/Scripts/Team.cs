using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team {
    private Color teamColor;
    private string teamName;
    private List<PlayerInfo> players;

    public Color TeamColor {
        get {
            return teamColor;
        }

        set {
            teamColor = value;
        }
    }

    public string TeamName
    {
        get
        {
            return teamName;
        }

        set
        {
            teamName = value;
        }
    }

    public Team(Color teamColor, string teamName) {
        this.TeamColor = teamColor;
        this.TeamName = teamName;
        players = new List<PlayerInfo>();
    } 

    public void addPlayerToTeam(PlayerInfo pInfo) {
        players.Add(pInfo);
    }

    public List<PlayerInfo> getTeamPlayers() {
        return players;
    } 

    public int CompareTo(Team that) {
        return this.TeamName.CompareTo(that.TeamName);
    }
}
