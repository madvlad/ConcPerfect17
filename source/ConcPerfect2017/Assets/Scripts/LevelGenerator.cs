using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public GameObject jumpSeparator;
    public GameObject startPrefab;
    public GameObject endPrefab;
    public List<GameObject> jumpList;
    public int courseJumpListSize;
    public int RandomSeed;

    private List<GameObject> CourseJumpList;
    private GameStateManager GameStateManager;
    private int CurrentJumpNumber = 1;

    void Start ()
    {
        GameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        GameStateManager.SetCourseJumpLimit(courseJumpListSize);
        if (RandomSeed != 0)
        {
            Random.InitState(RandomSeed);
        }

        // TODO :: Display this to the player
        Debug.Log("Seed being used: " + Random.seed);

        GameObject previousSnapPoint = InstantiateStartPoint();
        int lastNum = -1;
        for (int i = 0; i < courseJumpListSize; i++)
        {
            var nextJump = Random.Range(0, jumpList.Count);
            if (nextJump == lastNum)
            {
                i--;
            }
            else
            {
                previousSnapPoint = InstantiateJumpAtSnapPoint(previousSnapPoint, jumpList[nextJump]);
            }
            lastNum = nextJump;
            CurrentJumpNumber++;
        }

        InstantiateEndPoint(previousSnapPoint);
    }

    void BuildCourseIteratively()
    {
        GameObject previousSnapPoint = InstantiateStartPoint();

        foreach (var jump in CourseJumpList)
        {
            previousSnapPoint = InstantiateJumpAtSnapPoint(previousSnapPoint, jump);
        }

        InstantiateEndPoint(previousSnapPoint);
    }

    private GameObject InstantiateJumpAtSnapPoint(GameObject previousSnapPoint, GameObject jump)
    {
        var newJump = Instantiate(jump);
        var newJumpSeparator = Instantiate(jumpSeparator);
        newJump.GetComponent<SnapPointManager>().snapPointIn.transform.position = previousSnapPoint.transform.position;
        newJumpSeparator.transform.position = previousSnapPoint.transform.position;
        newJumpSeparator.GetComponent<JumpTrigger>().JumpNumber = CurrentJumpNumber;
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
