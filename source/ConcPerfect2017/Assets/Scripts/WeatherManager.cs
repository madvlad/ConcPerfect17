using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {
    public GameObject rain;

    private GameObject rainInstance;
    private bool parentSet = false;
    private bool isRaining = false;

	void Start () {
        var petesRainDance = Random.value;
        Debug.Log(petesRainDance);
        if (petesRainDance < 0.15f)
        {
            rainInstance = Instantiate(rain);
            isRaining = true;
        }
	}

    private void FixedUpdate()
    {
        if (!parentSet && isRaining)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null) {
                rainInstance.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                parentSet = true;
            }
        }
    }
}
