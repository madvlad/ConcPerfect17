using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            if (grounded && (Input.GetAxis("Horizontal") > 0.25 || Input.GetAxis("Vertical") > 0.25 || Input.GetAxis("Horizontal") < -0.25 || Input.GetAxis("Vertical") < -0.25))
            {
                var index = Random.Range(0, footstepSounds.Count);
                gameObject.GetComponent<AudioSource>().PlayOneShot(footstepSounds[index], ApplicationManager.sfxVolume);
            }
            timer = Random.Range(0.20f, 0.35f);
        }
    }
}