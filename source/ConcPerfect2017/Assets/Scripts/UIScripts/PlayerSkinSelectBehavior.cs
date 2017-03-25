using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinSelectBehavior : MonoBehaviour {

    public List<Material> playerSkins;
    public GameObject playerModel;
    public GameObject rotationModel;

    private int currentSkinIndex;
	
	void Start () {
        currentSkinIndex = ApplicationManager.PlayerModel;
        SetPlayerSkin();
	}

    void FixedUpdate()
    {
        if (rotationModel != null)
        {
            rotationModel.transform.Rotate(0, 75 * Time.deltaTime, 0);
        }
    }

    void SetPlayerSkin()
    {
        if (playerModel != null)
        {
            playerModel.GetComponent<SkinnedMeshRenderer>().material = playerSkins[currentSkinIndex];
            PlayerPrefs.SetInt("PlayerModel", currentSkinIndex);
            ApplicationManager.PlayerModel = currentSkinIndex;
        }
    }

    public void NextSkin()
    {
        currentSkinIndex++;

        if (currentSkinIndex >= playerSkins.Count)
        {
            currentSkinIndex = 0;
        }

        SetPlayerSkin();
    }

    public void PreviousSkin()
    {
        currentSkinIndex--;

        if (currentSkinIndex < 0)
        {
            currentSkinIndex = playerSkins.Count - 1;
        }

        SetPlayerSkin();
    }
}
