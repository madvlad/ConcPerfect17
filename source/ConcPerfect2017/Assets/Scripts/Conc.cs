using UnityEngine;
using System.Collections;

public class Conc : MonoBehaviour
{
    public ParticleSystem explosionParticleSystem;
    public GameObject devBubble;
    public bool ShowSphereCollider = false;
    public AudioClip timerSFX;
    public AudioClip explodeSFX;
    public AudioClip primeSFX;
    public AudioClip warningSFX;
    public float timer = 5f;
    private int BeepCount = 1;
    public bool exploded = false;

    void Start()
    {
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), GetComponent<Collider>());
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
        explosionParticleSystem.Emit(1);
        exploded = true;
        Invoke("Destroy", timer);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        EnactPush();
        PlayExplosionSound();
    }

    void EnactPush()
    {
        if (devBubble != null && ShowSphereCollider)
        {
            var bubble = Instantiate(devBubble);
            bubble.transform.position = transform.position;
        }
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
            if (BeepCount == (int)timer - 1)
            {
                Invoke("WarningBeep", 1.0f);
            }
            else
            {
                BeepCount++;
                Invoke("Beep", 1.0f);
            }
        }
    }

    void WarningBeep()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(warningSFX);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}