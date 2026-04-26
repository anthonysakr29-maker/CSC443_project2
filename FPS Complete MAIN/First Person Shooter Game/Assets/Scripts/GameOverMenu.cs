using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalMoneyText;

    private void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int finalMoney = PlayerPrefs.GetInt("FinalMoney", 0);

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