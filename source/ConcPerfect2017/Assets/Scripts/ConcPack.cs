using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcPack : MonoBehaviour {

    public float RespawnTime = 10f;
    public AudioClip PickupSound;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var currentConcer = other.GetComponent<Concer>();
            if (currentConcer.ConcCount < currentConcer.MaxConcCount)
            {
                AudioSource.PlayClipAtPoint(PickupSound, other.transform.position);
                var concsToAdd = currentConcer.MaxConcCount - currentConcer.ConcCount;
                currentConcer.SetConcCount(currentConcer.ConcCount + concsToAdd);
                DisablePack();
                Invoke("EnablePack", RespawnTime);
            }
        }
    }

    private void DisablePack()
    {
        gameObject.SetActive(false);
    }

    private void EnablePack()
    {
        gameObject.SetActive(true);
    }
}
