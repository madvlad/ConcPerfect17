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
    public List<Material> levelSkyboxList;
    public List<Texture> levelFloorTexture;
    public List<Texture> levelWallTexture;
    public List<AudioClip> levelMusicList;

    public AudioClip tutorialMusic;

    private List<GameObject> CourseJumpList;
    private GameStateManager GameStateManager;
    private int CurrentJumpNumber = 1;

    void Start ()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = ApplicationManager.musicVolume;
        GameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        if (!isServer)
        {
            ApplicationManager.currentLevel = GameStateManager.CurrentServerLevel;
            ApplicationManager.GameType = GameStateManager.CurrentGameType;
        }

        if (courseJumpListSize == 0)
        {
            courseJumpListSize = ApplicationManager.numberOfJumps;
        }

        if (ApplicationManager.currentLevel > 0)
        {
            SetCourseEnvironment(ApplicationManager.currentLevel);
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
                SetJumpTextures();
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

    public bool IsSetting = false;
    public bool HasSet = false;
    public bool StartWall = true;
    public int NumberOfCalls = 0;

    private void SetJumpTextures()
    {
        if (Application.isLoadingLevel && !IsSetting)
        {
            Invoke("SetJumpTextures", 0.1f);
            return;
        }
        IsSetting = true;
        var assetIndex = ApplicationManager.currentLevel - 1;
        var floors = GameObject.FindGameObjectsWithTag("Floor");
        foreach (var floor in floors)
        {
            if (floor.GetComponent<Renderer>() != null)
            {
                HasSet = true;
                if (StartWall)
                {
                    StartWall = false;
                    SetWallTextures();
                    SetMoveableTextures();
                }
                var MeshRenderer = floor.GetComponents(typeof(MeshRenderer))[0] as MeshRenderer;
                MeshRenderer.material.mainTexture = levelFloorTexture[assetIndex];
            }
            else if (!HasSet)
            {
                Invoke("SetJumpTextures", 0.1f);
            }
        }
    }

    private void SetWallTextures()
    {
        var walls = GameObject.FindGameObjectsWithTag("Wall");
        var assetIndex = ApplicationManager.currentLevel - 1;

        foreach (var wall in walls)
        {
            var MeshRenderer = wall.GetComponents(typeof(MeshRenderer))[0] as MeshRenderer;
            MeshRenderer.material.mainTexture = levelWallTexture[assetIndex];
        }
    }

    private void SetMoveableTextures()
    {
        var walls = GameObject.FindGameObjectsWithTag("Moveable");
        var assetIndex = ApplicationManager.currentLevel - 1;

        foreach (var wall in walls)
        {
            var MeshRenderer = wall.GetComponents(typeof(MeshRenderer))[0] as MeshRenderer;
            MeshRenderer.material.mainTexture = levelFloorTexture[assetIndex];
        }
    }

    private void SetCourseEnvironment(int currentLevel)
    {
        SetPossibleWeatherEvents(currentLevel);
        var assetIndex = currentLevel - 1;
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().clip = levelMusicList[assetIndex];
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();
        RenderSettings.skybox = levelSkyboxList[assetIndex];
        SetJumpTextures();
    }

    private void SetPossibleWeatherEvents(int currentLevel)
    {
        var weatherManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WeatherManager>();
        switch (currentLevel)
        {
            case 4:
                weatherManager.MakeItSnow();
                break;
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
        if (ApplicationManager.JumpsDifficultiesAllowed.Count == 0)
        {
            ApplicationManager.JumpsDifficultiesAllowed = new List<int> { 0, 1, 2, 3, 4 };
        }
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

            if (nextJump == lastNum || !ApplicationManager.JumpsDifficultiesAllowed.Contains(jumpList[nextJump].GetComponent<SnapPointManager>().jumpDifficulty))
            {
                i--;
            }
            else
            {
                previousSnapPoint = InstantiateJumpAtSnapPoint(previousSnapPoint, jumpList[nextJump]);
                lastNum = nextJump;
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
