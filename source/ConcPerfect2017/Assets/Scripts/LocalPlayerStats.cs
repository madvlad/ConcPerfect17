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
    public void RequestCourseJumpLimit() {
        if (!isLocalPlayer)
            return;
        CmdRequestCourseJumpLimt();
    }

		if (this.gameObject.GetComponent<LocalPlayerStats> () != null)
		{
			this.gameObject.GetComponent<LocalPlayerStats> ().UpdateTime ("Not Started");
		}
	}
	
	void Update () {
		
	}
		
	public void UpdateTime(string currentTimerTime)
	{
		if (!isLocalPlayer)
			return;
		CmdUpdateTime (netId, currentTimerTime);
	}
    [Command]
    public void CmdRequestCourseJumpLimt() {
        gameServerManager.GetCourseJumpLimit();
    }

	public void UpdateJump(int jump)
	{
		if (!isLocalPlayer)
			return;
		CmdUpdateJump (netId, jump);
	}
		
	[Command]
	public void CmdUpdateTime(NetworkInstanceId netId, string currentTimerTime)
	{
		gameServerManager.UpdatePlayerTime (netId, currentTimerTime);
	}

	[Command]
	public void CmdUpdateJump(NetworkInstanceId netId, int jump)
	{
		gameServerManager.UpdatePlayerJump (netId, jump);
	}
}
