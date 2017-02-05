using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelGenerator : NetworkBehaviour {
    public GameObject jumpSeparator;
    public GameObject raceStartPrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;
    public GameObject gameManager;
    public GameObject tutorialLevel;
    public List<GameObject> jumpList;
    public int courseJumpListSize;
    public int RandomSeed;

    public AudioClip tutorialMusic;

    private List<GameObject> CourseJumpList;
    private GameStateManager GameStateManager;
    private int CurrentJumpNumber = 1;

    void Start ()
    {
        GameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();

        if (courseJumpListSize == 0)
        {
            courseJumpListSize = ApplicationManager.numberOfJumps;
        }

        GameStateManager.SetCourseJumpLimit(courseJumpListSize);

        if (!isServer)
            return;

        // Casual Mode
        if (ApplicationManager.GameType == 0 || ApplicationManager.GameType == 2)
        {
            if (ApplicationManager.currentLevel > 0)
            {
                BuildCourseIteratively();
            }
            else
            {
                BuildRandomCourse();
            }
        }
        // Tutorial
        else if (ApplicationManager.GameType == 1)
        {
            BuildTutorialLevel();
        }
    }

    private void BuildTutorialLevel()
    {
        Instantiate(tutorialLevel);
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().clip = tutorialMusic;
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();
    }

    private void BuildRandomCourse()
    {
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

            if(!ApplicationManager.JumpsDifficultiesAllowed.Contains(jumpList[nextJump].GetComponent<SnapPointManager>().jumpDifficulty))
            {
                i--;
            }
            else if (nextJump == lastNum)
            {
                i--;
                lastNum = nextJump;
            }
            else
            {
                previousSnapPoint = InstantiateJumpAtSnapPoint(previousSnapPoint, jumpList[nextJump]);
                CurrentJumpNumber++;
            }
        }

        InstantiateEndPoint(previousSnapPoint);
    }

    void BuildCourseIteratively()
    {
        CourseJumpList = gameManager.GetComponent<LevelManager>().getLevel(ApplicationManager.currentLevel);
        GameStateManager.SetCourseJumpLimit(CourseJumpList.Count);
        GameObject previousSnapPoint = InstantiateStartPoint();

        foreach (var jump in CourseJumpList)
        {
            previousSnapPoint = InstantiateJumpAtSnapPoint(previousSnapPoint, jump);
            CurrentJumpNumber++;
        }

        InstantiateEndPoint(previousSnapPoint);
    }

    private GameObject InstantiateJumpAtSnapPoint(GameObject previousSnapPoint, GameObject jump)
    {
        var newJump = Instantiate(jump);
        var newJumpSeparator = Instantiate(jumpSeparator);
        newJump.GetComponent<NetworkTransform>().transform.position = previousSnapPoint.transform.position;
        newJump.GetComponent<SnapPointManager>().snapPointIn.transform.position = previousSnapPoint.transform.position;
        newJumpSeparator.GetComponent<NetworkTransform>().transform.position = previousSnapPoint.transform.position;
        newJumpSeparator.transform.position = previousSnapPoint.transform.position;
        newJumpSeparator.GetComponent<JumpTrigger>().JumpNumber = CurrentJumpNumber;
        newJumpSeparator.GetComponent<JumpTrigger>().JumpName = newJump.GetComponent<SnapPointManager>().jumpName;
        previousSnapPoint = newJump.GetComponent<SnapPointManager>().snapPointOut;
        NetworkServer.Spawn(newJump);
        NetworkServer.Spawn(newJumpSeparator);
        return previousSnapPoint;
    }

    private GameObject InstantiateStartPoint()
    {
        GameObject newStartPrefab;
        if (ApplicationManager.GameType == 0)
        {
            newStartPrefab = Instantiate(startPrefab);
        }
        else
        {
            newStartPrefab = Instantiate(raceStartPrefab);
        }
        NetworkServer.Spawn(newStartPrefab);
        GameObject previousSnapPoint = newStartPrefab.GetComponent<SnapPointManager>().snapPointOut;
        return previousSnapPoint;
    }

    private void InstantiateEndPoint(GameObject previousSnapPoint)
    {
        var newEndPrefab = Instantiate(endPrefab);
        newEndPrefab.GetComponent<NetworkTransform>().transform.position = previousSnapPoint.transform.position;
        NetworkServer.Spawn(newEndPrefab);
    }
}
