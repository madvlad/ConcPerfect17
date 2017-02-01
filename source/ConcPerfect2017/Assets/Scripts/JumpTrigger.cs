using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JumpTrigger : NetworkBehaviour {

    [SyncVar]
    public int JumpNumber;

    [SyncVar]
    public string JumpName;

    public AudioClip checkpointSound;

    private bool WasTriggered = false;
    private GameStateManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !WasTriggered)
        {
            WasTriggered = true;
            gameManager.SetJumpNumber(JumpNumber);
            gameManager.SetJumpName(JumpName);
            gameObject.GetComponent<AudioSource>().PlayOneShot(checkpointSound);
        }
    }
}
