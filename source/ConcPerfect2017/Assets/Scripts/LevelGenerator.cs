using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public List<GameObject> jumpList;
    public int jumpNumber;
    public int RandomSeed;

	void Start () {
        if (RandomSeed != 0)
        {
            Random.InitState(RandomSeed);
        }

        GameObject previousSnapPoint = null;

        for (int i = 0; i < jumpNumber; i++)
        {
            var nextJump = Random.Range(0, jumpList.Count);
            var newJump = Instantiate(jumpList[nextJump]);

            if (previousSnapPoint != null)
            {
                newJump.GetComponent<SnapPointManager>().snapPointIn.transform.position = previousSnapPoint.transform.position;
            }
            previousSnapPoint = newJump.GetComponent<SnapPointManager>().snapPointOut;
        }
	}
	
	void Update () {
		
	}
}
