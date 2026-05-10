using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLock : MonoBehaviour
{
    public static bool CanLockCursor = true;

    private void Start()
    {
        if (CanLockCursor)
            LockCursor();
    }

    private void Update()
    {
        if (!CanLockCursor) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}