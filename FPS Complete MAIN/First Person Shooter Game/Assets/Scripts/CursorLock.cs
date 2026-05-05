using UnityEngine;

public class CursorLock : MonoBehaviour
{
    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        // If user clicks, re-lock cursor
        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}