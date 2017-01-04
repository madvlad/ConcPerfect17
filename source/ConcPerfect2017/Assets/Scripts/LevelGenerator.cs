using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public GameObject startPrefab;
    public GameObject endPrefab;
    public List<GameObject> jumpList;
    private List<GameObject> courseJumpList;
    public int jumpNumber;
    public int RandomSeed;

	void Start ()
    {
        if (RandomSeed != 0)
        {
            Random.InitState(RandomSeed);
        }

        // TODO :: Display this to the player
        Debug.Log("Seed being used: " + Random.seed);

        GameObject previousSnapPoint = InstantiateStartPoint();

        for (int i = 0; i < jumpNumber; i++)
        {
            var nextJump = Random.Range(0, jumpList.Count);
            previousSnapPoint = InstantiateJumpAtSnapPoint(previousSnapPoint, jumpList[nextJump]);
        }

        InstantiateEndPoint(previousSnapPoint);
    }

    void BuildCourseIteratively()
    {
        GameObject previousSnapPoint = InstantiateStartPoint();

        foreach (var jump in courseJumpList)
        {
            previousSnapPoint = InstantiateJumpAtSnapPoint(previousSnapPoint, jump);
        }

        InstantiateEndPoint(previousSnapPoint);
    }

    private static GameObject InstantiateJumpAtSnapPoint(GameObject previousSnapPoint, GameObject jump)
    {
        var newJump = Instantiate(jump);
        newJump.GetComponent<SnapPointManager>().snapPointIn.transform.position = previousSnapPoint.transform.position;
        previousSnapPoint = newJump.GetComponent<SnapPointManager>().snapPointOut;
        return previousSnapPoint;
    }

    private GameObject InstantiateStartPoint()
    {
        var newStartPrefab = Instantiate(startPrefab);
        GameObject previousSnapPoint = newStartPrefab.GetComponent<SnapPointManager>().snapPointOut;
        return previousSnapPoint;
    }

    private void InstantiateEndPoint(GameObject previousSnapPoint)
    {
        var newEndPrefab = Instantiate(endPrefab);
        newEndPrefab.GetComponent<SnapPointManager>().snapPointIn.transform.position = previousSnapPoint.transform.position;
    }
}
