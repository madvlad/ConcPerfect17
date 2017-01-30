using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerStats : NetworkBehaviour {

	public struct PlayerStat
	{
		public string PlayerId;
		public int CurrentJump;
		public string CourseTime;
	}

	// TODO - Make This List Sync!
	public class SyncListPlayerStats : SyncListStruct<PlayerStat>
	{
		/*public bool ContainsStat(PlayerStat statToCompare)
		{
			foreach (PlayerStat s in this)
				if (s.PlayerId == statToCompare.PlayerId)
					return true;
			return false;
		}

		public PlayerStat GetStat(PlayerStat statToGet)
		{
		    foreach (PlayerStat s in this)
				if (s.PlayerId == statToGet.PlayerId) 
					return s;
			return new PlayerStat ();
		}

		public void RemoveStat(PlayerStat statToRemove)
		{
			PlayerStat s = GetStat (statToRemove);
			if (s.PlayerId != null)
				this.Remove (s);
		}

		public void AddStat (PlayerStat s)
		{
			this.Add (s);
		}*/
	}

	public SyncListPlayerStats playerStatsList = new SyncListPlayerStats();
	public PlayerStat localPlayerStats;

	private GameStateManager gameManager;

	void Start()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
		localPlayerStats = new PlayerStat ();
		localPlayerStats.PlayerId = "Player" + netId;
		playerStatsList.Add (localPlayerStats);
	}
		
	public List<string> GetPlayerStats()
	{
		playerStatsList.Remove (localPlayerStats);
		localPlayerStats.CurrentJump = gameManager.GetCurrentJumpNumber ();
		localPlayerStats.CourseTime = gameManager.GetCurrentTime ();
		playerStatsList.Add (localPlayerStats);

		List<string> playerStats = new List<string> ();
		foreach (PlayerStat stat in playerStatsList) {
			playerStats.Add (stat.PlayerId + " : " + stat.CurrentJump + "/" + gameManager.GetCourseJumpLimit () + " : " + stat.CourseTime);
		}
		return playerStats;
	}
}
