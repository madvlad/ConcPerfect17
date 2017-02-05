using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class GameServerManager : NetworkBehaviour {
	private GameStateManager gameManager;
    

	public struct PlayerStat
	{
		public NetworkInstanceId PlayerId;
        public string Nickname;
		public int CurrentJump;
		public string CurrentTimerTime;
	}

	public class ListPlayerStats : List<PlayerStat>
	{
		public void RemoveStatByPlayerId(NetworkInstanceId netId)
		{
			PlayerStat s = GetStatByPlayerId (netId);
			if (s.PlayerId != null)
				this.Remove (s);
		}


		public PlayerStat GetStatByPlayerId(NetworkInstanceId netId)
		{
			foreach (PlayerStat s in this)
				if (s.PlayerId == netId) 
					return s;
			return new PlayerStat ();
		}

        public bool HasStatWithPlayerId(NetworkInstanceId netId) {
            foreach (PlayerStat s in this)
                if (s.PlayerId == netId)
                    return true;
            return false; 
        }

	}

	public ListPlayerStats playerStatsList = new ListPlayerStats();

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
	}

	void Update () {
		if (!isServer)
			return;

		string playerStats = "";
		foreach (PlayerStat stat in playerStatsList) {
			playerStats += stat.Nickname + " ; " + stat.CurrentJump + "/" + gameManager.GetCourseJumpLimit () + " ; " + stat.CurrentTimerTime + "%";
		}
			
		gameManager.RpcUpdatePlayerStats (playerStats);
	}

    public PlayerStat getPlayerStat(NetworkInstanceId netId) {
        PlayerStat playerStat;
        if (!playerStatsList.HasStatWithPlayerId(netId))
            playerStat = InitializeNewPlayer(netId);
        else
            playerStat = playerStatsList.GetStatByPlayerId(netId);

        return playerStat;
    }

    public void UpdatePlayerNickname(NetworkInstanceId netId, string nickname) {
        PlayerStat playerStat = getPlayerStat(netId);
        playerStatsList.RemoveStatByPlayerId(netId);
        playerStat.Nickname = nickname;
        playerStatsList.Add(playerStat);
        foreach (PlayerStat stat in playerStatsList) {
            gameManager.RpcUpdatePlayerNickname(stat.PlayerId, stat.Nickname);
        }
    }

	public void UpdatePlayerTime(NetworkInstanceId netId, string playerTime)
	{
        PlayerStat playerStat = getPlayerStat(netId);
        playerStatsList.RemoveStatByPlayerId (netId);
		playerStat.CurrentTimerTime = playerTime;
		playerStatsList.Add (playerStat);

	}

	public void UpdatePlayerJump(NetworkInstanceId netId, int jump)
	{
		PlayerStat playerStat = playerStatsList.GetStatByPlayerId (netId);
		if (playerStat.PlayerId == null)
			playerStat = InitializeNewPlayer (netId);
		playerStatsList.RemoveStatByPlayerId (netId);
		playerStat.CurrentJump = jump;
		playerStatsList.Add (playerStat);
	}

	public PlayerStat InitializeNewPlayer(NetworkInstanceId netId)
	{
		PlayerStat newPlayer = new PlayerStat ();
		newPlayer.PlayerId = netId;
        newPlayer.Nickname = "0xD15EA5E";
		newPlayer.CurrentTimerTime = "Not Started";
		newPlayer.CurrentJump = 0;
		playerStatsList.Add (newPlayer);
		return newPlayer;
	}

    public void GetCourseJumpLimit() {
        gameManager.RpcUpdateCourseJumpLimit(gameManager.GetCourseJumpLimit());
    }

    public void RequestPlayerNicknames() {
        foreach (PlayerStat stat in playerStatsList) {
            gameManager.RpcUpdatePlayerNickname(stat.PlayerId, stat.Nickname);
        }
    }
}
