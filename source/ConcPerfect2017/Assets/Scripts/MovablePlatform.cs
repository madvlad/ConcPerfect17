using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour {

    public List<GameObject> WayPoints;
    public float speed;

    private GameObject nextWayPoint;
    private int idx = 0;

    void Start()
    {

    }

	void Update () {
        nextWayPoint = WayPoints[idx];
        if (Vector3.Distance(gameObject.transform.position, nextWayPoint.transform.position) < 0.5)
        {
            if (idx == WayPoints.Count - 1)
            {
                idx = 0;
            }
            else
            {
                idx++;
            }

            nextWayPoint = WayPoints[idx];
        }

        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextWayPoint.transform.position, speed);
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Conc"))
        {
            collision.gameObject.transform.parent = gameObject.transform;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Conc"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
