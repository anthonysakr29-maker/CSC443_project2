using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuClickRaycaster : MonoBehaviour
{
    [SerializeField] private Camera menuCamera;
    [SerializeField] private float rayDistance = 100f;

    private void Awake()
    {
        if (menuCamera == null)
            menuCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = menuCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            MainMenu3DButton button = hit.collider.GetComponentInParent<MainMenu3DButton>();

            if (button != null)
            {
                button.Click();
            }
        }
    }
}