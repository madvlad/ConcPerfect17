using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackSelectorScript : MonoBehaviour {
	public List<GameObject> levelPacks;

	private int index = 0;

	// Use this for initialization
	void Start () {
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
}
