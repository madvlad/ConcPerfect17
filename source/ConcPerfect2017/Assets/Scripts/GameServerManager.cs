using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class GameServerManager : NetworkBehaviour {
	private GameStateManager gameManager;
    private BeaconManager beaconManager;
    private bool wait = false;

	public Dictionary<NetworkInstanceId, PlayerInfo> currentPlayers = new Dictionary<NetworkInstanceId, PlayerInfo> ();
	public Dictionary<NetworkInstanceId, string> playerNicknames = new Dictionary<NetworkInstanceId, string> ();
    public Dictionary<NetworkInstanceId, int> playerSkins = new Dictionary<NetworkInstanceId, int>();
    public Dictionary<Team, int> teamBeaconScores = new Dictionary<Team, int>();

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        beaconManager = GameObject.FindGameObjectWithTag("BeaconManager").GetComponent<BeaconManager>();
    }

	void Update () {
		if (!isServer)
			return;

		CleanupDisconnectedPlayers ();

        if (!wait)
        {
            wait = true;
            string playerInfo = "";
            List<PlayerInfo> players = currentPlayers.Select(kvp => kvp.Value).ToList();
            players.Sort((s1, s2) => s1.CompareTo(s2));
            Team currentDisplayTeam = null;
            TeamManager tm = GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>();
            foreach (PlayerInfo pInfo in players)
            {
                if (ApplicationManager.GameType == GameTypes.ConcminationGameType) {
                    if (pInfo.CurrentTeam != null && (currentDisplayTeam == null || !currentDisplayTeam.TeamName.Equals(pInfo.CurrentTeam))) {
                        currentDisplayTeam = tm.GetTeamByName(pInfo.CurrentTeam);
                        playerInfo += currentDisplayTeam.TeamName + " ; " + teamBeaconScores[currentDisplayTeam] + " % ";
                    }
                    playerInfo += pInfo.PrintPlayerInfoConcminationMode();

                    List<Team> winners = beaconManager.GetWinners();
                    if (winners != null && winners.Count > 0) {
                        string winnerstr = "";
                        foreach (Team t in winners) {
                            winnerstr += t.TeamName + ",";
                        }
                        gameManager.RpcUpdateConcminationWinners(winnerstr.TrimEnd(','));
                    }
                }  else
                    playerInfo += pInfo.PrintPlayerInfoRaceMode();
            }
            gameManager.RpcUpdatePlayerInfo(playerInfo);
            Invoke("StopWaiting", 1.0f);
        }
    }

    public void UpdatePlayerTeam(string teamName, PlayerInfo player) {
        if (currentPlayers.ContainsKey(player.PlayerId)) {
            PlayerInfo pInfo = currentPlayers[player.PlayerId];
            pInfo.CurrentTeam = teamName;
            currentPlayers.Remove(player.PlayerId);
            currentPlayers[player.PlayerId] = pInfo;
        }

        // Re-Initialize the teamscores when someone joins a Team
        teamBeaconScores = beaconManager.GetTeamBeaconCount();
    }

    void StopWaiting()
    {
        wait = false;
    }

    
    public void CleanupDisconnectedPlayers() {
		List<NetworkInstanceId> idsToRemove = new List<NetworkInstanceId> ();
		foreach (NetworkInstanceId id in currentPlayers.Keys) {
			if (ClientScene.FindLocalObject(id) == null) {
				idsToRemove.Add (id);
			}
		}
			
		foreach (NetworkInstanceId id in idsToRemove) {
			currentPlayers.Remove (id);
		}
	}

    public void UpdatePlayerNickname(NetworkInstanceId netId, string nickname, int playerModel) {
		RegisterPlayer (netId, nickname, playerModel);
		foreach (KeyValuePair<NetworkInstanceId, string> entry in playerNicknames) {
			gameManager.RpcUpdatePlayerNickname(entry.Key, entry.Value);
        }

        foreach (KeyValuePair<NetworkInstanceId, int> entry in playerSkins)
        {
            gameManager.RpcUpdatePlayerSkins(entry.Key, entry.Value);
        }
    }

    public void UpdatePlayerBeaconsOwned(NetworkInstanceId netId) {
        int beaconCount = beaconManager.GetUserBeaconCount(netId);
        if (currentPlayers.ContainsKey(netId)) {
            PlayerInfo pInfo = currentPlayers[netId];
            pInfo.BeaconsCaptured = beaconCount;
            currentPlayers.Remove(netId);
            currentPlayers[netId] = pInfo;
        }
        teamBeaconScores = beaconManager.GetTeamBeaconCount();
    }

	public void UpdatePlayerTime(NetworkInstanceId netId, string playerTime)
	{
		if (currentPlayers.ContainsKey(netId)) {
			PlayerInfo pInfo = currentPlayers [netId];
			if (pInfo.BestTime == "- -") {
				pInfo.BestTime = playerTime;
			} else {
				pInfo.BestTime = (pInfo.BestTime.CompareTo (playerTime) == 1) ? playerTime : pInfo.BestTime;
			}
			pInfo.TimesCompleted++;
			currentPlayers.Remove (netId);
			currentPlayers [netId] = pInfo;
		}
	}

	public void UpdatePlayerStatus(NetworkInstanceId netId, string status) {
		if (currentPlayers.ContainsKey(netId)) {
			PlayerInfo pInfo = currentPlayers [netId];
			pInfo.Status = status;
			currentPlayers.Remove (netId);
			currentPlayers [netId] = pInfo;
		}
	}

	public void UpdatePlayerJump(NetworkInstanceId netId, int jump) {
		if (currentPlayers.ContainsKey (netId)) {
			PlayerInfo pInfo = currentPlayers [netId];
			pInfo.CurrentJump = jump;
			currentPlayers.Remove (netId);
			currentPlayers[netId] = pInfo;
		} 
	}
		
	public void RegisterPlayer(NetworkInstanceId netId, string nickname, int playerModel) {
		if (playerNicknames.ContainsKey (netId)) {
			playerNicknames.Remove (netId);
		}

		if (currentPlayers.ContainsKey (netId)) {
			currentPlayers.Remove (netId);
		}

		string newNick = nickname;
		int i = 1;
		while (playerNicknames.ContainsValue(newNick)) {
			newNick = nickname + (i++);
		}
		playerNicknames [netId] = newNick;
        playerSkins[netId] = playerModel;
		PlayerInfo pInfo = new PlayerInfo();
		pInfo.PlayerId = netId;
		pInfo.Nickname = playerNicknames [netId];
		pInfo.Status = "Not Started";
		pInfo.CurrentJump = 0;
        pInfo.CourseJumpLimit = gameManager.GetCourseJumpLimit();
		pInfo.BestTime = "- -";
		pInfo.TimesCompleted = 0;
        pInfo.BeaconsCaptured = 0;
        pInfo.PlayerModel = playerModel;
		currentPlayers [netId] = pInfo;

    }

    public void GetCourseJumpLimit() {
        gameManager.RpcUpdateCourseJumpLimit(gameManager.GetCourseJumpLimit());
    }

    public void RequestPlayerNicknames() {
		foreach (KeyValuePair<NetworkInstanceId, string> entry in playerNicknames) {
			gameManager.RpcUpdatePlayerNickname(entry.Key, entry.Value);
        }
    }

    public void RequestPlayerSkins()
    {
        foreach (KeyValuePair<NetworkInstanceId, int> entry in playerSkins)
        {
            gameManager.RpcUpdatePlayerSkins(entry.Key, entry.Value);
        }

    }

}
