﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public List<GameObject> level1;
    public List<GameObject> level2;
    public List<GameObject> level3;
    public List<GameObject> level4;
    public List<GameObject> level5;

    internal List<GameObject> getLevel(int currentLevel)
    {
        switch(currentLevel)
        {
            case 1:
                return level1;
            case 2:
                return level2;
            case 3:
                return level3;
            case 4:
                return level4;
            case 5:
                return level5;
            default:
                return null;
        }
    }
}
