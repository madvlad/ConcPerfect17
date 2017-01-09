using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour {
    public int JumpNumber;
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
            gameObject.GetComponent<AudioSource>().PlayOneShot(checkpointSound);
        }
    }
}
