using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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

    public void TurnTheDangSunOff()
    {
        if (GameObject.FindGameObjectWithTag("Sun") != null)
        {
            var sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
            sun.flare = null;
        }
    }

    public void MakePeteDoHisRainDance()
    {
        var petesRainDance = Random.value;

        if (petesRainDance < 0.15f)
        {
            MakeItRain();
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
            var localPlayer = GetLocalPlayerObject();
            if (localPlayer != null) {
                weatherEventInstance.transform.parent = localPlayer.transform;
                parentSet = true;
            }
        }
    }

    private GameObject GetLocalPlayerObject()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject playerObject = null;
        foreach (GameObject obj in playerObjects)
        {
            if (obj.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerObject = obj;
            }
        }

        return playerObject;
    }
}
