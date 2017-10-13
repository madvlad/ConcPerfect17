using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCountdown : MonoBehaviour {
    public GameObject CountdownPanel;
    float timer = 15;
    bool started = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!started)
            return;
        timer -= Time.deltaTime;
        CountdownPanel.GetComponent<Text>().text = Mathf.CeilToInt(timer).ToString();
        if (timer < 0) {
            StopCountdown();
        }
	}

    public void StartCountDown() {
        started = true;
        timer = 15;
        CountdownPanel.GetComponent<Text>().enabled = true;
    }

    void StopCountdown() {
        started = false;
        CountdownPanel.GetComponent<Text>().enabled = false;
    }
}
