using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public List<GameObject> level1;
    public List<GameObject> level2;
    public List<GameObject> level3;
    public List<GameObject> level4;
    public List<GameObject> level5;
	public List<GameObject> level6;
    public List<GameObject> level7;
    public List<GameObject> level8;
    public List<GameObject> level9;
    public List<GameObject> level10;
    public List<GameObject> level11;
    public List<GameObject> level12;
    public List<GameObject> level13;
    public List<GameObject> level14;
    public List<GameObject> level15;
    public List<GameObject> level16;


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
			case 6:
				return level6;
            case 7:
                return level7;
            case 8:
                return level8;
            case 9:
                return level9;
            default:
                return level9;
        }
    }
}
