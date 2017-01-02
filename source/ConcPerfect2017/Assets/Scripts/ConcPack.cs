using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcPack : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        var currentConcCount = other.GetComponent<Concer>().ConcCount;
        if (currentConcCount < 3)
        {
            var concsToAdd = 3 - other.GetComponent<Concer>().ConcCount;
            other.GetComponent<Concer>().ConcCount += concsToAdd;
            Destroy(this.gameObject);
        }
    }
}
