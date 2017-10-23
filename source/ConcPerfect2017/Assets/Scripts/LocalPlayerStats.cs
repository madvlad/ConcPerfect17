﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayerStats : NetworkBehaviour
{
    // Server Objects
    public GameServerManager gameServerManager;
    public GameObject ChatObject;

    void Start()
    {
        if (isServer)
            gameServerManager = GameObject.FindGameObjectWithTag("GameServerManager").GetComponent<GameServerManager>();

        ChatObject = GameObject.FindGameObjectWithTag("Chat");

        if (this.gameObject.GetComponent<LocalPlayerStats>() != null)
        {
            this.gameObject.GetComponent<LocalPlayerStats>().UpdateNickname();
        }
    }

    void Update()
    {

    }

    public void UpdateNickname()
    {
        if (!isLocalPlayer)
            return;
        CmdUpdateNickname(netId, ApplicationManager.Nickname, ApplicationManager.PlayerModel);
    }

    public void UpdateTime(string currentTimerTime)
    {
        if (!isLocalPlayer)
            return;
        CmdUpdateTime(netId, currentTimerTime);
    }

    public void UpdateStatus(string status)
    {
        if (!isLocalPlayer)
            return;
        CmdUpdateStatus(netId, status);
    }

    public void UpdateCapturedBeacons(string capturingTeam, string nickname, NetworkInstanceId playerId, NetworkInstanceId beaconId, float time)
    {
        if (!isLocalPlayer)
            return;
        CmdUpdateCapturedBeacons(capturingTeam, nickname, playerId, beaconId, time);
    }

    public void UpdateJump(int jump)
    {
        if (!isLocalPlayer)
            return;
        CmdUpdateJump(netId, jump);
    }

    public void RequestCourseJumpLimit()
    {
        if (!isLocalPlayer)
            return;
        CmdRequestCourseJumpLimt();
    }

    public void RequestPlayerNicknames()
    {
        if (!isLocalPlayer)
            return;
        CmdRequestPlayerNicknames();
    }

    public void RequestPlayerSkins()
    {
        if (!isLocalPlayer)
            return;
        CmdRequestPlayerSkins();
    }

    public void RequestPlayerTeamSkins()
    {
        if (!isLocalPlayer)
            return;
        CmdRequestTeamPlayerSkins();
    }

    [Command]
    public void CmdRequestCourseJumpLimt()
    {
        gameServerManager.GetCourseJumpLimit();
    }

    [Command]
    public void CmdUpdateTime(NetworkInstanceId netId, string currentTimerTime)
    {
        gameServerManager.UpdatePlayerTime(netId, currentTimerTime);
    }

    [Command]
    public void CmdUpdateStatus(NetworkInstanceId netId, string status)
    {
        gameServerManager.UpdatePlayerStatus(netId, status);
    }

    [Command]
    public void CmdUpdateJump(NetworkInstanceId netId, int jump)
    {
        gameServerManager.UpdatePlayerJump(netId, jump);
    }

    [Command]
    public void CmdUpdateCapturedBeacons(string capturingTeam, string nickname, NetworkInstanceId playerId, NetworkInstanceId beaconId, float time)
    {
        gameServerManager.UpdatePlayerBeaconsOwned(netId);
        GameObject.FindGameObjectWithTag("BeaconManager").GetComponent<BeaconManager>().UpdateBeaconCapturer(capturingTeam, nickname, playerId, beaconId, time);
    }

    [Command]
    public void CmdUpdateNickname(NetworkInstanceId netId, string nickname, int playerModel)
    {
        Debug.Log("Update nickname command sent");
        gameServerManager.UpdatePlayerNickname(netId, nickname, playerModel);
    }

    [Command]
    public void CmdRequestPlayerNicknames()
    {
        gameServerManager.RequestPlayerNicknames();
    }

    [Command]
    public void CmdRequestPlayerSkins()
    {
        GameObject.FindGameObjectWithTag("GameServerManager").GetComponent<GameServerManager>().RequestPlayerSkins();
    }

    [Command]
    public void CmdRequestTeamPlayerSkins()
    {
        GameObject.FindGameObjectWithTag("GameServerManager").GetComponent<GameServerManager>().RequestTeamSkins();
    }

    [Command]
    public void CmdAddPlayerToTeam(string teamName, PlayerInfo pInfo, int skinNumber)
    {
        GameObject.FindGameObjectWithTag("TeamManager").GetComponent<TeamManager>().AddPlayerToTeam(teamName, pInfo, skinNumber);
    }
}
