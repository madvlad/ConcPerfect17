using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public GameObject destination;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = destination.transform.position;
            other.transform.rotation = Quaternion.Euler(destination.transform.rotation.x, destination.transform.rotation.y, destination.transform.rotation.z);
        }
    }
}