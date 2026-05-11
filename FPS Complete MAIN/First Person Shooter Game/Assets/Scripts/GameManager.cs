using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Values")]
    public int score;
    public int money;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddReward(int scoreAmount, int moneyAmount)
    {
        score += scoreAmount;
        money += moneyAmount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (moneyText != null)
            moneyText.text = "$ " + money;
    }

    public void SaveFinalStats()
    {
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("FinalMoney", money);
        PlayerPrefs.Save();
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount) return false;

        money -= amount;
        UpdateUI();
        return true;
    }
}