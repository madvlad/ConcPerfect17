using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimerOnTrigger : MonoBehaviour {
    public GameObject gameManager;
    public GameObject startLabel;
    public bool SwitchToOn;
    public AudioClip GameEndSound;

    private GameStateManager gameStateManager;

    void Start()
    {
        gameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameStateManager.TimerIsRunning && SwitchToOn)
        {
            gameStateManager.SetTimerIsRunning(SwitchToOn);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            startLabel.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (gameStateManager.TimerIsRunning && !SwitchToOn)
        {
            gameStateManager.SetTimerIsRunning(SwitchToOn);
            gameObject.GetComponent<AudioSource>().PlayOneShot(GameEndSound);
            GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Stop();
        }
    }
}
