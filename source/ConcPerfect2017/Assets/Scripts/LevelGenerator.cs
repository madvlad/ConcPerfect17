﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelGenerator : NetworkBehaviour {
    public GameObject jumpSeparator;
    public GameObject raceStartPrefab;
    public GameObject premadeRaceStartPrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;
    public GameObject gameManager;
    public GameObject tutorialLevel;
    public List<GameObject> levelList;
    public List<GameObject> jumpList;
    public List<GameObject> concminationLevelList;
    public int courseJumpListSize;
    public int RandomSeed;
    public List<Material> levelSkyboxList;
    public List<Texture> levelFloorTexture;
    public List<Texture> levelWallTexture;
    public List<AudioClip> levelMusicList;
    public Texture lavaTexture;

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

        if (ApplicationManager.GameType == GameTypes.CasualGameType || ApplicationManager.GameType == GameTypes.RaceGameType || ApplicationManager.GameType == GameTypes.ConcminationGameType)
        {
            if (ApplicationManager.currentLevel > 0)
            {
                var parentLevel = SpawnLevelPrefab(ApplicationManager.currentLevel);

                if (isServer && ApplicationManager.GameType == GameTypes.ConcminationGameType)
                {
                    SpawnConcminationStartPrefab(ApplicationManager.currentLevel, parentLevel.transform);
                }
            }
            else
            {
                if (!isServer)
                    return;
                BuildRandomCourse();
            }
        }
        else if (ApplicationManager.GameType == GameTypes.TutorialGameType)
        {
            BuildTutorialLevel();
        }
        SetSFXVolume();
        SetMusicVolume();
    }

    private void SetSFXVolume()
    {
        var sfxObjects = GameObject.FindGameObjectsWithTag("SFX");
        foreach (var sfx in sfxObjects)
        {
            sfx.GetComponent<AudioSource>().volume = ApplicationManager.sfxVolume;
        }
    }

    private void SetMusicVolume()
    {
        var musicObjects = GameObject.FindGameObjectsWithTag("Music");
        foreach (var music in musicObjects)
        {
            music.GetComponent<AudioSource>().volume = ApplicationManager.musicVolume;
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
                    if (ApplicationManager.currentLevel == 5)
                    {
                        SetLavaTextures();
                    }
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

    private void SetLavaTextures()
    {
        var walls = GameObject.FindGameObjectsWithTag("Lava");

        foreach (var wall in walls)
        {
            var MeshRenderer = wall.GetComponents(typeof(MeshRenderer))[0] as MeshRenderer;
            MeshRenderer.material.mainTexture = lavaTexture;
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
        //SetJumpTextures();
    }

    private void SetPossibleRain()
    {
        var weatherManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WeatherManager>();
        weatherManager.MakePeteDoHisRainDance();
    }

    private void SetPossibleWeatherEvents(int currentLevel)
    {
        var weatherManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WeatherManager>();
        switch (currentLevel)
        {
            case 3:
                weatherManager.TurnTheDangSunOff();
                break;
            case 4:
                weatherManager.MakeItSnow();
                break;
            case 5:
                weatherManager.TurnOnThunderstorm();
                break;
        }
    }

    private void BuildTutorialLevel()
    {
        Instantiate(tutorialLevel);
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().clip = tutorialMusic;
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();
    }

    private GameObject SpawnLevelPrefab(int currentLevel)
    {
        GameStateManager.SetCourseJumpLimit(gameManager.GetComponent<LevelManager>().getLevel(ApplicationManager.currentLevel).Count);
        var level = Instantiate(levelList[currentLevel - 1]);

        if (isServer && ApplicationManager.GameType == GameTypes.RaceGameType)
        {
            var raceStart = Instantiate(premadeRaceStartPrefab);
            NetworkServer.Spawn(raceStart);
        }

        return level;
    }

    private void SpawnConcminationStartPrefab(int currentLevel, Transform parent)
    {
        var concminationStart = Instantiate(concminationLevelList[currentLevel - 1], parent);
        NetworkServer.Spawn(concminationStart);
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
        SetPossibleRain();
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
        if (ApplicationManager.GameType == GameTypes.CasualGameType || ApplicationManager.GameType == GameTypes.ConcminationGameType)
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
