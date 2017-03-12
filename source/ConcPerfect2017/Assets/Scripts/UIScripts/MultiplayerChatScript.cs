using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MultiplayerChatScript : NetworkBehaviour {

    public GameObject chatMessages;
    public GameObject myMessage;
    public GameObject myInputField;
    public GameObject chatPanel;

    private Text chatMessagesText;
    private string nickname;
    private bool FirstMessage = true;
    private float chatFadeTimer = 5.0f;


    void Start () {
        
	}

    public override void OnStartLocalPlayer()
    {
        myInputField = GameObject.FindGameObjectWithTag("ChatInputField");
        myMessage = GameObject.FindGameObjectWithTag("ChatInputMessage");
        chatMessages = GameObject.FindGameObjectWithTag("ChatInputIncomingMessages");
        chatPanel = GameObject.FindGameObjectWithTag("Chat");
        chatMessagesText = chatMessages.GetComponent<Text>();
        nickname = ApplicationManager.Nickname;
        CmdChatMessage("\n<color=\"#ffff00ff\">" + ApplicationManager.Nickname + " has joined the game.</color>");
    }

    public override void OnNetworkDestroy()
    {
        if (!isLocalPlayer)
            return;

        CmdChatMessage("\n<color=\"#ffff00ff\">" + ApplicationManager.Nickname + " has left the game.</color>");
    }

    internal void SendStartMessage()
    {
        if (!isLocalPlayer)
            return;

        CmdChatMessage("\n<color=\"#ff0000ff\">" + ApplicationManager.Nickname + " started the course!</color>");
    }


    internal void SendFinishMessage(string timeString)
    {
        if (!isLocalPlayer)
            return;

        CmdChatMessage("\n<color=\"#ff0000ff\">" + ApplicationManager.Nickname + " finished the course in " + timeString + "!</color>");
    }

    internal void SendRecordFinishMessage(string timeString)
    {
        if (!isLocalPlayer)
            return;

        CmdChatMessage("\n<color=\"#ff00ffff\">" + ApplicationManager.Nickname + " got a new personal record on this course with " + timeString + "!!</color>");
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        chatFadeTimer -= Time.deltaTime;

        if (chatFadeTimer <= 0 && !myInputField.GetComponent<InputField>().isFocused)
        {
            chatPanel.GetComponent<Canvas>().enabled = false;
        }
    }

    void ChatBoxUpdated()
    {
        chatFadeTimer = 5.0f;
        chatPanel.GetComponent<Canvas>().enabled = true;
    }

    void Update () {
        if (!isLocalPlayer)
            return;

        if (Input.GetButton("ChatButton"))
        {
            ChatBoxUpdated();
            GetLocalPlayerObject().GetComponent<FirstPersonDrifter>().SetEscaped(true);
            myInputField.GetComponent<InputField>().Select();
            myInputField.GetComponent<InputField>().ActivateInputField();
        }

        var currentMessage = myMessage.GetComponent<Text>().text;

		if (Input.GetButton("Submit") || Input.GetButton("Cancel"))
        {
            if (!string.IsNullOrEmpty(currentMessage))
            {
                var message = "\n" + ApplicationManager.Nickname + " says, \"<color=\"#00ffffff\">" + currentMessage + "</color>\"";
                CmdChatMessage(message);
            }

            myInputField.GetComponent<InputField>().text = string.Empty;
            myInputField.GetComponent<InputField>().DeactivateInputField();
            GetLocalPlayerObject().GetComponent<FirstPersonDrifter>().SetEscaped(false);
        }
	}

    [Command]
    void CmdChatMessage(string message)
    {
        var recipients = GameObject.FindGameObjectsWithTag("Player");

        foreach(var recipient in recipients)
        {
            recipient.GetComponent<MultiplayerChatScript>().RpcReceiveChatMessage(message);
        }
    }

    [ClientRpc]
    void RpcReceiveChatMessage(string message)
    {
        ReceiveMessage(message);
    }

    public void ReceiveMessage(string message)
    {
        if (!isLocalPlayer)
            return;

        if (FirstMessage)
        {
            chatMessagesText.text = "\n<color=\"#ffff00ff\">Welcome to ConcPerfect 2017.</color>";
            FirstMessage = false;
        }
        else
        {
            chatMessagesText.text += message;
        }

        ChatBoxUpdated();
    }

    private GameObject GetLocalPlayerObject()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject playerObject = null;
        foreach (GameObject obj in playerObjects)
        {
            if (obj.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerObject = obj;
            }
        }

        return playerObject;
    }
}
