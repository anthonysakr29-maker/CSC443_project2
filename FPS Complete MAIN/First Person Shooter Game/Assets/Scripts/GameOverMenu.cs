using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalMoneyText;
    [SerializeField] private TextMeshProUGUI waveText;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int finalMoney = PlayerPrefs.GetInt("FinalMoney", 0);
        int finalWave = PlayerPrefs.GetInt("FinalWave", 1);

        waveText.text = "Wave Reached: " + finalWave;
        finalScoreText.text = "Final Score: " + finalScore;
        finalMoneyText.text = "Money Earned: $" + finalMoney;
    }

    public void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}