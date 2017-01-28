﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerStats : NetworkBehaviour {

	private static readonly Object statsLock = new Object();

	public struct PlayerStat
	{
		public string PlayerId;
		public int CurrentJump;
		public string CourseTime;
	}

	// TODO - Make This List Sync!
	public class SyncListPlayerStats : SyncListStruct<PlayerStat>
	{
		public bool ContainsStat(PlayerStat statToCompare)
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
		}
	}

	public SyncListPlayerStats playerStatsList = new SyncListPlayerStats();
	public PlayerStat localPlayerStats = new PlayerStat();

	private GameStateManager gameManager;

	void Start()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
	}

	void Update()
	{
		localPlayerStats.PlayerId = "Player" + netId;
		localPlayerStats.CurrentJump = gameManager.GetCurrentJumpNumber ();
		localPlayerStats.CourseTime = gameManager.GetCurrentTime ();

		UpdatePlayer (localPlayerStats);
	}
		
	// TODO - Make this list sync
	public void UpdatePlayer(PlayerStat stats)
	{
		if (!playerStatsList.ContainsStat(stats)) {
			playerStatsList.AddStat(stats);
		} else {
			playerStatsList.RemoveStat(stats);
			playerStatsList.AddStat(stats);
		}
	}


	public List<string> GetPlayerStats()
	{
		List<string> playerStats = new List<string> ();
		foreach (PlayerStat stat in playerStatsList) {
			playerStats.Add (stat.PlayerId + " : " + stat.CurrentJump + "/" + gameManager.GetCourseJumpLimit () + " : " + stat.CourseTime);
		}
		return playerStats;
	}
}
