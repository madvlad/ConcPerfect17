﻿// by @torahhorse

using UnityEngine;
using System.Collections;

public class LockMouse : MonoBehaviour
{
    private bool currentlyLocked = true;

    void Start()
    {
        LockCursor(true);
    }

    void Update()
    {
        // lock when mouse is clicked
        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0.0f)
        {
            currentlyLocked = true;
            LockCursor(currentlyLocked);
        }

        // unlock when escape is hit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentlyLocked = !currentlyLocked;
            LockCursor(currentlyLocked);
        }
    }

    public void LockCursor(bool lockCursor)
    {
        Cursor.visible = lockCursor;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}