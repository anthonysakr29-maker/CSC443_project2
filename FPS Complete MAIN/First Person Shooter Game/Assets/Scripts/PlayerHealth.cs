using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image damageFlashImage;

    [Header("Damage Flash")]
    [SerializeField] private float flashDuration = 0.15f;

    private int currentHealth;
    private bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (damageFlashImage != null)
        {
            damageFlashImage.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (healthText != null)
            healthText.text = currentHealth + " / " + maxHealth;
    }

    private System.Collections.IEnumerator DamageFlash()
    {
        if (damageFlashImage == null) yield break;

        damageFlashImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        damageFlashImage.gameObject.SetActive(false);
    }

    private void Die()
    {
        isDead = true;

        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();

        if (spawner != null)
        {
            PlayerPrefs.SetInt("FinalWave", spawner.GetCurrentWave());
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveFinalStats();
        }

        SceneManager.LoadScene("GameOver");
    }
}