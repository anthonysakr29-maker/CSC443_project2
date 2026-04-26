using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject controlsPanel;

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenControls()
    {
        controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}