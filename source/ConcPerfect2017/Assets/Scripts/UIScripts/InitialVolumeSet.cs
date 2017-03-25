using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialVolumeSet : MonoBehaviour {
    
	void Start () {
        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            var sfxObjects = GameObject.FindGameObjectsWithTag("SFX");
            foreach (var sfx in sfxObjects)
            {
                sfx.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SfxVolume");
            }
        }
    }
}
