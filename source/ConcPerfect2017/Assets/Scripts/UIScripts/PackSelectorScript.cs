using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackSelectorScript : MonoBehaviour {
	private List<GameObject> levelPacks;
    public List<GameObject> racePacks;
    public List<GameObject> concPacks;
    public GameObject customPack;

	private int index = 0;

	// Use this for initialization
	void Start () {
        levelPacks = racePacks;
		index = 0;
		levelPacks[0].SetActive(true);
	}

	public void ForwardOnePage()
	{
		levelPacks[index].SetActive(false);

		index++;

		if (index == levelPacks.Count)
			index = 0;

		levelPacks[index].SetActive(true);
	}

	public void BackOnePage()
	{
		levelPacks[index].SetActive(false);

		index--;

		if (index < 0)
			index = levelPacks.Count - 1;

		levelPacks[index].SetActive(true);
	}

    public void ChangeGameTypeLevels(int gameType) {
        levelPacks[index].SetActive(false);
        index = 0;
        if (gameType == GameTypes.CasualGameType || gameType == GameTypes.RaceGameType) {
            levelPacks = racePacks;
            customPack.SetActive(true);
        } else if (gameType == GameTypes.ConcminationGameType) {
            levelPacks = concPacks;
            customPack.SetActive(false);
        }
        levelPacks[index].SetActive(true);
    }
}
