using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Nickname : MonoBehaviour {
    private NetworkInstanceId playerId;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        if (ClientScene.FindLocalObject(playerId) == null) {
            Destroy(this.gameObject);
            return;
        }
         
        if (GetComponent<MeshRenderer>().enabled) {
            this.gameObject.transform.position = getPlayerPosition() + new Vector3(0, 1, 0);
            this.gameObject.transform.rotation = Camera.main.transform.rotation;
            this.gameObject.transform.LookAt(2 * this.gameObject.transform.position - Camera.main.transform.position);
        }
	}

    public void SetNickname(string nickname) {
        GetComponent<TextMesh>().text = nickname;
    }

    public void SetPlayerId(NetworkInstanceId netId) {
        this.playerId = netId;
        GetComponent<MeshRenderer>().enabled = true;
    }

    public Vector3 getPlayerPosition() {
        return ClientScene.FindLocalObject(playerId).transform.position;
    }

    public void SetDisplayNickname(bool shouldDisplay) {
        GetComponent<MeshRenderer>().enabled = shouldDisplay;
    }
}
