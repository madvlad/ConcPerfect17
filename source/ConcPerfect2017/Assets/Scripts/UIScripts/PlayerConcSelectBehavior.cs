using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConcSelectBehavior : MonoBehaviour {

    public List<Material> concMaterials;
    public List<Mesh> concMeshes;
    public GameObject concModel;
    public GameObject rotationModel;

    private int currentSkinIndex;
	
	void Start () {
        currentSkinIndex = ApplicationManager.ConcModel;
        SetPlayerSkin();
    }

    void FixedUpdate()
    {
        if (rotationModel != null)
        {
            rotationModel.transform.Rotate(0, 0, 75 * Time.deltaTime);
        }
    }

    void SetPlayerSkin()
    {
        if (concModel != null)
        {
            concModel.GetComponent<MeshRenderer>().material = concMaterials[currentSkinIndex];
            concModel.GetComponent<MeshFilter>().mesh = concMeshes[currentSkinIndex];
            PlayerPrefs.SetInt("ConcModel", currentSkinIndex);
            ApplicationManager.ConcModel = currentSkinIndex;
        }
    }

    public void NextSkin()
    {
        currentSkinIndex++;

        if (currentSkinIndex >= concMaterials.Count)
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
            currentSkinIndex = concMaterials.Count - 1;
        }

        SetPlayerSkin();
    }
}
