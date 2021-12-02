using UnityEngine;
using System.Collections;

public class MMouseLock : MonoBehaviour
{
    public KeyCode escapeKey = KeyCode.Escape;

    public IInputManager inputManager;

    private void Start()
    {
        inputManager = transform.parent.GetChild(0).gameObject.GetComponent<IInputManager>();
    }

    void Update()
    {
        if (inputManager.LMB)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (inputManager.ESC)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}