// by @torahhorse

using UnityEngine;
using System.Collections;

public class LockMouse : MonoBehaviour
{
    void Start()
    {
        LockCursor(true);
    }

    void Update()
    {
        // lock when mouse is clicked
        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0.0f)
        {
            LockCursor(true);
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