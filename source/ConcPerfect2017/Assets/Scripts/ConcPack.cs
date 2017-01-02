using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcPack : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        var currentConcer = other.GetComponent<Concer>();
        if (currentConcer.ConcCount < currentConcer.MaxConcCount)
        {
            var concsToAdd = currentConcer.MaxConcCount - currentConcer.ConcCount;
            currentConcer.ConcCount += concsToAdd;
            Destroy(this.gameObject);
            // TODO :: Add pickup sound
        }
    }
}
