using UnityEngine;

public class MainMenu3DButton : MonoBehaviour
{
    public enum ButtonType
    {
        Play,
        Controls,
        Quit
    }

    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private ButtonType buttonType;

    public void Click()
    {
        if (mainMenu == null)
        {
            Debug.LogWarning("MainMenu reference missing on " + name);
            return;
        }

        switch (buttonType)
        {
            case ButtonType.Play:
                mainMenu.Play();
                break;

            case ButtonType.Controls:
                mainMenu.OpenControls();
                break;

            case ButtonType.Quit:
                mainMenu.Quit();
                break;
        }
    }
}