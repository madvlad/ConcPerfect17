using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimerOnTrigger : MonoBehaviour {
    public GameObject gameManager;
    public bool SwitchToOn;
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
        }
        else if (gameStateManager.TimerIsRunning && !SwitchToOn)
        {
            gameStateManager.SetTimerIsRunning(SwitchToOn);
        }
    }
}
