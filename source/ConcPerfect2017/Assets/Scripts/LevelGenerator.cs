using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public List<GameObject> jumpList;

	void Start () {
        GameObject previousSnapPoint = null;
		foreach(var jump in jumpList)
        {
            var newJump = Instantiate(jump);
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
