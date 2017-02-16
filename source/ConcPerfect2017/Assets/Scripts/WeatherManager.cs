using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class WeatherManager : MonoBehaviour {
    public GameObject rain;
    public GameObject snow;
    public Material rainSkybox;
    public List<AudioClip> thunderSounds;

    private GameObject weatherEventInstance;
    private bool parentSet = false;
    private bool isRaining = false;
    private bool isThunderStorming;

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

    public void TurnOnThunderstorm()
    {
        isThunderStorming = true;
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

    private bool lightningOn = false;
    private float timer = 30.0f;

    private void FixedUpdate()
    {
        if (isThunderStorming)
        {
            if (lightningOn)
            {
                Camera.main.GetComponent<Bloom>().bloomThreshold = 0.5f;
                Camera.main.GetComponent<Bloom>().bloomIntensity = 0.2f;

                var thunderSoundIdx = Random.Range(0, 3);
                GetComponent<AudioSource>().PlayOneShot(thunderSounds[thunderSoundIdx], ApplicationManager.sfxVolume);

                lightningOn = false;
            }

            var lightning = Random.value;
            timer -= Time.deltaTime;
            if (lightning < 0.5f && timer < 0.0f)
            {
                Camera.main.GetComponent<Bloom>().bloomThreshold = 0.0f;
                Camera.main.GetComponent<Bloom>().bloomIntensity = 50.0f;
                lightningOn = true;
                timer = 30.0f;
            }
            else if (timer < 0.0f)
            {
                timer = 30.0f;
            }
        }
        
        if (!parentSet && isRaining)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null) {
                weatherEventInstance.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                parentSet = true;
            }
        }
    }
}
