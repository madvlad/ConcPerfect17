using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public GameObject destination;
    public bool StopMomentum = true;

    public bool IsSecret = false;
    public AudioClip SecretMusic;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = destination.transform.position;
            other.transform.rotation = Quaternion.Euler(destination.transform.rotation.x, destination.transform.rotation.y, destination.transform.rotation.z);

            if (StopMomentum)
            {
                other.gameObject.GetComponent<ImpactReceiver>().ZeroImpact();
            }

            if (IsSecret)
            {
                GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().clip = SecretMusic;
                GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();
            }
        }
    }
}