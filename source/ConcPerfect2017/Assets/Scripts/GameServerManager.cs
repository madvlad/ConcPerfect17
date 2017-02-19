using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class GameServerManager : NetworkBehaviour {
	private GameStateManager gameManager;
    

	public struct PlayerScore
	{
		public PlayerInfo pInfo;
		public string CurrentTimerTime;

		public int CompareTo(PlayerScore that)
		{
			int timeCompare = this.CurrentTimerTime.CompareTo (that.CurrentTimerTime);
			return (timeCompare == 0) ? this.pInfo.Nickname.CompareTo (that.pInfo.Nickname) : timeCompare;
		}
	}

	public struct PlayerInfo
	{
		public NetworkInstanceId PlayerId;
		public string Nickname;
		public string Status;
		public int CurrentJump;
	}

	public class ListPlayerScores : List<PlayerScore>
	{
		public void RemoveStatByPlayerId(NetworkInstanceId netId)
		{
			PlayerScore s = GetStatByPlayerId (netId);
			if (s.pInfo.PlayerId != null)
				this.Remove (s);
		}

		public PlayerScore GetStatByPlayerId(NetworkInstanceId netId)
		{
			foreach (PlayerScore s in this)
				if (s.pInfo.PlayerId == netId) 
					return s;
			return new PlayerScore ();
		}

        public bool HasStatWithPlayerId(NetworkInstanceId netId) {
            foreach (PlayerScore s in this)
                if (s.pInfo.PlayerId == netId)
                    return true;
            return false; 
        }
	}

	public ListPlayerScores playerScoresList = new ListPlayerScores();
	public Dictionary<NetworkInstanceId, PlayerInfo> currentPlayers = new Dictionary<NetworkInstanceId, PlayerInfo> ();
	public Dictionary<NetworkInstanceId, string> playerNicknames = new Dictionary<NetworkInstanceId, string> ();

	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
	}

	void Update () {
		if (!isServer)
			return;


		string playerScores = "";
		foreach (PlayerScore stat in playerScoresList) {
			playerScores += stat.pInfo.Nickname + " ; " + stat.CurrentTimerTime + "%";
		}

		string playerInfo = ""; 
		foreach (KeyValuePair<NetworkInstanceId, PlayerInfo> entry in currentPlayers) {
			playerInfo += entry.Value.Nickname + " ; " + entry.Value.Status + " ; " + entry.Value.CurrentJump + "/" + gameManager.GetCourseJumpLimit () + " % " ;
		}
			
		gameManager.RpcUpdatePlayerScores (playerScores);
		gameManager.RpcUpdatePlayerInfo (playerInfo);
	}

    public void UpdatePlayerNickname(NetworkInstanceId netId, string nickname) {
		RegisterPlayer (netId, nickname);
		foreach (KeyValuePair<NetworkInstanceId, string> entry in playerNicknames) {
			gameManager.RpcUpdatePlayerNickname(entry.Key, entry.Value);
        }
    }

	public void UpdatePlayerTime(NetworkInstanceId netId, string playerTime)
	{
		PlayerScore stat = new PlayerScore ();
		stat.pInfo = currentPlayers [netId];
		stat.CurrentTimerTime = playerTime;
		playerScoresList.Add(stat);
		playerScoresList.Sort ((s1, s2) => s1.CompareTo (s2));
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
		
	public void RegisterPlayer(NetworkInstanceId netId, string nickname) {
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
		PlayerInfo pInfo = new PlayerInfo();
		pInfo.PlayerId = netId;
		pInfo.Nickname = playerNicknames [netId];
		pInfo.Status = "Not Started";
		pInfo.CurrentJump = 0;
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
}
