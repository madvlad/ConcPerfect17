using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcPack : MonoBehaviour {

    public float RespawnTime = 10f;
    public AudioClip PickupSound;

    void OnTriggerEnter(Collider other)
    {
        var currentConcer = other.GetComponent<Concer>();
        if (currentConcer.ConcCount < currentConcer.MaxConcCount)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(PickupSound);
            var concsToAdd = currentConcer.MaxConcCount - currentConcer.ConcCount;
            currentConcer.ConcCount += concsToAdd;
            DisablePack();
            Invoke("EnablePack", RespawnTime);
        }
    }

    private void DisablePack()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void EnablePack()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
