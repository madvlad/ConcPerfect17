﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MouseLook : MonoBehaviour
{

    public enum RotationAxes { MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseX;
    public bool invertY = false;

    private float sensitivityX = 10F;
    private float sensitivityY = 9F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -85F;
    public float maximumY = 85F;

    float rotationX = 0F;
    float rotationY = 0F;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    public float framesOfSmoothing = 5;

    Quaternion originalRotation;

    void Start()
    {
        if (!(gameObject == GetLocalPlayerObject() || gameObject.transform.parent == GetLocalPlayerObject().transform))
            return;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (!(gameObject == GetLocalPlayerObject() || gameObject.transform.parent == GetLocalPlayerObject().transform))
            return;

        invertY = ApplicationManager.invertYAxis;

        sensitivityX = ApplicationManager.mouseSensitivity;
        sensitivityY = ApplicationManager.mouseSensitivity;

        if (axes == RotationAxes.MouseX)
        {
            rotAverageX = 0f;

            rotationX += Input.GetAxis("Mouse X") * sensitivityX * Time.timeScale;

            rotArrayX.Add(rotationX);

            if (rotArrayX.Count >= framesOfSmoothing)
            {
                rotArrayX.RemoveAt(0);
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }
            rotAverageX /= rotArrayX.Count;
            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotAverageY = 0f;

            float invertFlag = 1f;
            if (invertY)
            {
                invertFlag = -1f;
            }
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY * invertFlag * Time.timeScale;

            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            rotArrayY.Add(rotationY);

            if (rotArrayY.Count >= framesOfSmoothing)
            {
                rotArrayY.RemoveAt(0);
            }
            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            rotAverageY /= rotArrayY.Count;

            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            transform.localRotation = originalRotation * yQuaternion;
        }
    }

    public void SetSensitivity(float s)
    {
        sensitivityX = s;
        sensitivityY = s;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }

    private GameObject GetLocalPlayerObject()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject playerObject = null;
        foreach (GameObject obj in playerObjects)
        {
            if (obj.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerObject = obj;
            }
        }

        return playerObject;
    }
}
