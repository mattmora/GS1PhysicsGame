using UnityEngine;
using System.Collections;

public class MMouseLock : MonoBehaviour
{
    public KeyCode escapeKey = KeyCode.Escape;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(escapeKey))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}