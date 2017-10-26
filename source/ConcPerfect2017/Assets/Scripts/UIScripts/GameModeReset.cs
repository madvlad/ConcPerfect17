using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ApplicationManager.GameType = GameTypes.CasualGameType;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
