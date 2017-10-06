using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Conc : NetworkBehaviour
{
    public ParticleSystem explosionParticleSystem;
    public ParticleSystem explosionFlashParticleSystem;
    public ParticleSystem explosionEmbersParticleSystem;
    public GameObject devBubble;
    public bool ShowSphereCollider = false;
    public AudioClip timerSFX;
    public AudioClip explodeSFX;
    public AudioClip primeSFX;
    public AudioClip warningSFX;
    public float timer = 5f;
    public float timeLeft = 5f;
    public bool exploded = false;

    public GameObject owner;
    public bool remote = false;
    private GameObject playerObject;
    private int BeepCount = 1;

    void Start()
    {
        if (remote)
            return;

        var playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject obj in playerObjects)
        {
            if (obj.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerObject = obj;
            }
        }

        Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>());
        Invoke("Explode", timer);
        if (timerSFX)
        {
            if (gameObject.GetComponent<AudioSource>())
            {
                AudioSource.PlayClipAtPoint(primeSFX, playerObject.transform.position, ApplicationManager.sfxVolume);
                Invoke("Beep", 1.0f);
            }
        }
    }

    void FixedUpdate()
    {
        timeLeft -= Time.deltaTime;
    }

    void Explode()
    {
        explosionParticleSystem.Emit(1);
        explosionFlashParticleSystem.Emit(1);
        explosionEmbersParticleSystem.Emit(100);
        exploded = true;
        Invoke("Destroy", timer);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        EnactPush();
        PlayExplosionSound();
    }

    public void HarmlesslyExplode()
    {
        explosionParticleSystem.Emit(1);
        explosionFlashParticleSystem.Emit(1);
        explosionEmbersParticleSystem.Emit(100);
        exploded = true;
        Invoke("Destroy", 0.5f);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
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
        if (explodeSFX && explodeSFX != null && playerObject != null)
        {
            AudioSource.PlayClipAtPoint(explodeSFX, playerObject.transform.position, ApplicationManager.sfxVolume);
        }
    }

    void Beep()
    {
        if (!exploded)
        {
            AudioSource.PlayClipAtPoint(timerSFX, playerObject.transform.position, ApplicationManager.sfxVolume);
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
        AudioSource.PlayClipAtPoint(warningSFX, playerObject.transform.position, ApplicationManager.sfxVolume);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
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