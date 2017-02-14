using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {
    public GameObject rain;
    public GameObject snow;
    public Material rainSkybox;

    private GameObject weatherEventInstance;
    private bool parentSet = false;
    private bool isRaining = false;

    public void MakeItRain()
    {
        weatherEventInstance = Instantiate(rain);
        RenderSettings.skybox = rainSkybox;
        isRaining = true;
    }

    public void MakeItSnow()
    {
        weatherEventInstance = Instantiate(snow);
        isRaining = true;
    }

	void Start () {
        if (ApplicationManager.currentLevel == 0)
        {
            var petesRainDance = Random.value;

            if (petesRainDance < 0.15f)
            {
                MakeItRain();
            }
        }
	}

    private void FixedUpdate()
    {
        if (!parentSet && isRaining)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null) {
                weatherEventInstance.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                parentSet = true;
            }
        }
    }
}
