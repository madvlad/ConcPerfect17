using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinSelectBehavior : MonoBehaviour {

    public List<Material> playerSkins;
    public GameObject playerModel;
    public GameObject rotationModel;

    private int currentSkinIndex;
	
	void Start () {
		
	}

    void FixedUpdate()
    {
        rotationModel.transform.Rotate(0, 75 * Time.deltaTime, 0);
    }

    void SetPlayerSkin(Material newSkin)
    {
        playerModel.GetComponent<SkinnedMeshRenderer>().material = newSkin;
    }

    public void NextSkin()
    {
        currentSkinIndex++;

        if (currentSkinIndex >= playerSkins.Count)
        {
            currentSkinIndex = 0;
            SetPlayerSkin(playerSkins[0]);
        }

        SetPlayerSkin(playerSkins[currentSkinIndex]);
    }

    public void PreviousSkin()
    {
        currentSkinIndex--;

        if (currentSkinIndex < 0)
        {
            currentSkinIndex = playerSkins.Count - 1;
        }

        SetPlayerSkin(playerSkins[currentSkinIndex]);
    }
}
