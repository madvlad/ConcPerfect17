using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MultiplayerChatScript : NetworkBehaviour {

    public GameObject chatMessages;
    public GameObject myMessage;
    public GameObject myInputField;

    private Text chatMessagesText;
    private bool FirstMessage = true;

	void Start () {
        
	}

    public override void OnStartLocalPlayer()
    {
        myInputField = GameObject.FindGameObjectWithTag("ChatInputField");
        myMessage = GameObject.FindGameObjectWithTag("ChatInputMessage");
        chatMessages = GameObject.FindGameObjectWithTag("ChatInputIncomingMessages");
        chatMessagesText = chatMessages.GetComponent<Text>();
        CmdChatMessage("\n" + ApplicationManager.Nickname + " has joined the game.");
    }

    public override void OnNetworkDestroy()
    {
        CmdChatMessage("\n" + ApplicationManager.Nickname + " has left the game.");
    }

    void Update () {
        if (!isLocalPlayer)
            return;

        if (Input.GetButton("ChatButton"))
        {
            GetLocalPlayerObject().GetComponent<FirstPersonDrifter>().SetEscaped(true);
            myInputField.GetComponent<InputField>().Select();
            myInputField.GetComponent<InputField>().ActivateInputField();
        }

        var currentMessage = myMessage.GetComponent<Text>().text;

		if (Input.GetButton("Submit"))
        {
            if (!string.IsNullOrEmpty(currentMessage))
            {
                var message = "\n" + ApplicationManager.Nickname + " says, \"" + currentMessage + "\"";
                CmdChatMessage(message);
            }

            myInputField.GetComponent<InputField>().text = string.Empty;
            myInputField.GetComponent<InputField>().DeactivateInputField();
            GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonDrifter>().SetEscaped(false);
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
            chatMessagesText.text = "\nWelcome to ConcPerfect 2017.";
            FirstMessage = false;
        }
        else
        {
            chatMessagesText.text += message;
        }
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
