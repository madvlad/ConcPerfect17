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

    private bool wait = false;

	public class ListPlayerScores : List<PlayerScoreRaceMode>
	{
		public void RemoveStatByPlayerId(NetworkInstanceId netId)
		{
			PlayerScoreRaceMode s = GetStatByPlayerId (netId);
			if (s.PInfo.PlayerId != null)
				this.Remove (s);
		}

		public PlayerScoreRaceMode GetStatByPlayerId(NetworkInstanceId netId)
		{
			foreach (PlayerScoreRaceMode s in this)
				if (s.PInfo.PlayerId == netId) 
					return s;
			return null;
		}

        public bool HasStatWithPlayerId(NetworkInstanceId netId) {
            foreach (PlayerScoreRaceMode s in this)
                if (s.PInfo.PlayerId == netId)
                    return true;
            return false; 
        }
	}

	public ListPlayerScores playerScoresList = new ListPlayerScores();
	public Dictionary<NetworkInstanceId, PlayerInfo> currentPlayers = new Dictionary<NetworkInstanceId, PlayerInfo> ();
	public Dictionary<NetworkInstanceId, string> playerNicknames = new Dictionary<NetworkInstanceId, string> ();
    public Dictionary<NetworkInstanceId, int> playerSkins = new Dictionary<NetworkInstanceId, int>();

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
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
            foreach (PlayerInfo pInfo in players)
            {
                playerInfo += pInfo.PlayerId.ToString() + " ; " + pInfo.Nickname + " ; " + pInfo.Status + " ; " + pInfo.CurrentJump + "/" + gameManager.GetCourseJumpLimit() + " ; " + pInfo.BestTime + " ; " + pInfo.TimesCompleted + " % ";
            }
            gameManager.RpcUpdatePlayerInfo(playerInfo);
            Invoke("StopWaiting", 1.0f);
        }
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

	public void UpdatePlayerTime(NetworkInstanceId netId, string playerTime)
	{
        PlayerScoreRaceMode stat = new PlayerScoreRaceMode {
            PInfo = currentPlayers[netId],
            CurrentTimerTime = playerTime
        };
        playerScoresList.Add(stat);
		playerScoresList.Sort ((s1, s2) => s1.CompareTo (s2));

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
		pInfo.BestTime = "- -";
		pInfo.TimesCompleted = 0;
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
