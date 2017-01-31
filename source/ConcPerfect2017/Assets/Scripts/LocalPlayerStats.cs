using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayerStats : NetworkBehaviour {

	// Server Objects
	public GameServerManager gameServerManager;

	void Start () {
		if (isServer)
			gameServerManager = GameObject.FindGameObjectWithTag ("GameServerManager").GetComponent<GameServerManager> ();
	}
	
	void Update () {
		
	}
		
	public void UpdateTime(float currentTimerTime)
	{
		if (!isLocalPlayer)
			return;
		CmdUpdateTime (netId, currentTimerTime);
	}

	public void UpdateJump(int jump)
	{
		if (!isLocalPlayer)
			return;
		CmdUpdateJump (netId, jump);
	}
		
	[Command]
	public void CmdUpdateTime(NetworkInstanceId netId, float currentTimerTime)
	{
		gameServerManager.UpdatePlayerTime (netId, currentTimerTime);
	}

	[Command]
	public void CmdUpdateJump(NetworkInstanceId netId, int jump)
	{
		gameServerManager.UpdatePlayerJump (netId, jump);
	}
}
