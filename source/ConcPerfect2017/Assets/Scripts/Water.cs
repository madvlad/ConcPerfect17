using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public Color underwaterFogColor = Color.blue;
    public float underwaterFogDensity = 0.12f;

    private Color previousFogColor;
    private float previousFogDensity;
    private bool previousFogEnabled;

    void Start()
    {
        previousFogColor = RenderSettings.fogColor;
        previousFogDensity = RenderSettings.fogDensity;
        previousFogEnabled = RenderSettings.fog;
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonDrifter player = other.GetComponent<FirstPersonDrifter>();
            if (player == null)
                return;

            RenderSettings.fog = true;
            RenderSettings.fogDensity = underwaterFogDensity;
            RenderSettings.fogColor = underwaterFogColor;

            player.underwater = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonDrifter player = other.GetComponent<FirstPersonDrifter>();
            if (player == null)
                return;

            RenderSettings.fogDensity = previousFogDensity;
            RenderSettings.fogColor = previousFogColor;
            RenderSettings.fog = previousFogEnabled;

            player.underwater = false;
        }
    }
}
