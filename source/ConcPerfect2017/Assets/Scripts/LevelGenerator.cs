using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelGenerator : NetworkBehaviour {
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
        if (!isServer)
          return;
        GameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();

        if (courseJumpListSize == 0)
        {
            courseJumpListSize = ApplicationManager.numberOfJumps;
        }

        GameStateManager.SetCourseJumpLimit(courseJumpListSize);
        RandomSeed = ApplicationManager.randomSeed;

        if (RandomSeed != 0)
        {
            Random.InitState(RandomSeed);
        }

        GameStateManager.SetJumpSeed(Random.seed);

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
                CurrentJumpNumber++;
            }
            lastNum = nextJump;
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
        newJump.GetComponent<NetworkTransform> ().transform.position = previousSnapPoint.transform.position;
        newJump.GetComponent<SnapPointManager>().snapPointIn.transform.position = previousSnapPoint.transform.position;
        newJumpSeparator.GetComponent<NetworkTransform>().transform.position = previousSnapPoint.transform.position;
        newJumpSeparator.transform.position = previousSnapPoint.transform.position;
        newJumpSeparator.GetComponent<JumpTrigger>().JumpNumber = CurrentJumpNumber;
        previousSnapPoint = newJump.GetComponent<SnapPointManager>().snapPointOut;
        NetworkServer.Spawn(newJump);
        return previousSnapPoint;
    }

    private GameObject InstantiateStartPoint()
    {
        var newStartPrefab = Instantiate(startPrefab);
        newStartPrefab.GetComponent<NetworkTransform> ().transform.position = newStartPrefab.transform.position;
        GameObject previousSnapPoint = newStartPrefab.GetComponent<SnapPointManager>().snapPointOut;
        NetworkServer.Spawn(newStartPrefab);
        return previousSnapPoint;
    }

    private void InstantiateEndPoint(GameObject previousSnapPoint)
    {
        var newEndPrefab = Instantiate(endPrefab);
        newEndPrefab.GetComponent<NetworkTransform> ().transform.position = previousSnapPoint.transform.position;
        newEndPrefab.GetComponent<SnapPointManager>().snapPointIn.transform.position = previousSnapPoint.transform.position;
        NetworkServer.Spawn(newEndPrefab);
    }
}
