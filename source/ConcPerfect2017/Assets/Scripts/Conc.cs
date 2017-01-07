using UnityEngine;
using System.Collections;

public class Conc : MonoBehaviour
{
    public AudioClip timerSFX;
    public AudioClip explodeSFX;
    public AudioClip primeSFX;
    public float timer = 4.0f;
    public bool exploded = false;

    void Start()
    {
        Invoke("Explode", timer);
        if (timerSFX)
        {
            if (gameObject.GetComponent<AudioSource>())
            {
                gameObject.GetComponent<AudioSource>().PlayOneShot(primeSFX);
                Invoke("Beep", 1.0f);
            }
        }
    }

    void Explode()
    {
        exploded = true;
        Invoke("Destroy", timer);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        EnactPush();
        PlayExplosionSound();
    }

    void EnactPush()
    {
        var colliders = Physics.OverlapSphere(transform.position, 7.0f);
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                var receiver = hit.GetComponent<ImpactReceiver>();
                if (receiver)
                {
                    var dir = hit.transform.position - transform.position;
                    float force;
                    if (dir.magnitude == 0)
                    {
                        force = Mathf.Clamp(100.0f, 0, 100.0f);
                        dir = hit.GetComponent<CharacterController>().velocity * hit.GetComponent<CharacterController>().velocity.sqrMagnitude;
                    }
                    else
                    {
                        force = Mathf.Clamp(100.0f, 0, 100.0f) * dir.magnitude;
                    }
                    receiver.AddImpact(dir, force);
                }
            }
        }
    }

    void PlayExplosionSound()
    {
        if (explodeSFX)
        {
            if (gameObject.GetComponent<AudioSource>())
            {
                gameObject.GetComponent<AudioSource>().PlayOneShot(explodeSFX);
            }
            else
            {
                AudioSource.PlayClipAtPoint(explodeSFX, gameObject.transform.position);
            }
        }
    }

    void Beep()
    {
        if (!exploded)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(timerSFX);
            Invoke("Beep", 1.0f);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}