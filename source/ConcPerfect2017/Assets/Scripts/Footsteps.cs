using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Footsteps : MonoBehaviour
{
    public List<AudioClip> footstepSounds;
    public float timer = 0.25f;
    
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            var grounded = GetComponent<FirstPersonDrifter>().grounded;
            if (grounded && (Input.GetAxis("Horizontal") > 0.25 || Input.GetAxis("Vertical") > 0.25 || Input.GetAxis("Horizontal") < -0.25 || Input.GetAxis("Vertical") < -0.25) && !GetLocalPlayerObject().GetComponent<FirstPersonDrifter>().IsEscaped())
            {
                var index = Random.Range(0, footstepSounds.Count);
                gameObject.GetComponent<AudioSource>().PlayOneShot(footstepSounds[index], ApplicationManager.sfxVolume);
            }
            timer = Random.Range(0.20f, 0.35f);
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