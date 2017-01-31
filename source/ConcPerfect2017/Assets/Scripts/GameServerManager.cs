using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class GameServerManager : NetworkBehaviour {

	GameStateManager gameManager;

	public struct PlayerStat
	{
		public string PlayerId;
		public int CurrentJump;
		public float CurrentTimerTime;
	}

	public class ListPlayerStats : List<PlayerStat>
	{
		public void RemoveStatByPlayerId(string netId)
		{
			PlayerStat s = GetStatByPlayerId (netId);
			if (s.PlayerId != null)
				this.Remove (s);
		}


		public PlayerStat GetStatByPlayerId(string netId)
		{
			foreach (PlayerStat s in this)
				if (s.PlayerId == netId) 
					return s;
			return new PlayerStat ();
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
			playerStats += stat.PlayerId + " : " + stat.CurrentJump + "/" + gameManager.GetCourseJumpLimit () + " : " + stat.CurrentTimerTime + "%";
		}
			
		gameManager.RpcUpdatePlayerStats (playerStats);

	}

	public void UpdatePlayerTime(NetworkInstanceId netId, float playerTime)
	{
		PlayerStat playerStat = playerStatsList.GetStatByPlayerId (netId.ToString());
		if (playerStat.PlayerId == null)
			playerStat = InitializeNewPlayer (netId.ToString());
		playerStatsList.RemoveStatByPlayerId (netId.ToString());
		playerStat.CurrentTimerTime = playerTime;
		playerStatsList.Add (playerStat);

	}

	public void UpdatePlayerJump(NetworkInstanceId netId, int jump)
	{
		PlayerStat playerStat = playerStatsList.GetStatByPlayerId (netId.ToString());
		if (playerStat.PlayerId == null)
			playerStat = InitializeNewPlayer (netId.ToString());
		playerStatsList.RemoveStatByPlayerId (netId.ToString());
		playerStat.CurrentJump = jump;
		playerStatsList.Add (playerStat);
	}

	public PlayerStat InitializeNewPlayer(string netId)
	{
		PlayerStat newPlayer = new PlayerStat ();
		newPlayer.PlayerId = netId;
		newPlayer.CurrentTimerTime = 0;
		newPlayer.CurrentJump = 0;
		playerStatsList.Add (newPlayer);
		return newPlayer;
	}



}
