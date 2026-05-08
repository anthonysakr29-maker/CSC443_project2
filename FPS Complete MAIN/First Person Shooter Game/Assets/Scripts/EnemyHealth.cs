using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IPoolable
{
    [SerializeField] private int startingHealth = 3;
    

    [Header("Rewards")]
    [SerializeField] private int scoreReward = 100;
    [SerializeField] private int moneyReward = 100;

    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color hitColor = Color.white;
    [SerializeField] private float flashTime = 0.08f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private GameObject deathEffectPrefab;

    private int currentHealth;
    public int ScoreReward => scoreReward;
    public int MoneyReward => moneyReward;

    public event Action<EnemyHealth> OnDied;
    public event Action<int, int> OnHealthChanged;

    private Color[] originalColors;

    private void Awake()
    {
        currentHealth = startingHealth;

        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(Flash());
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, startingHealth);
        OnHealthChanged?.Invoke(currentHealth, startingHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    private System.Collections.IEnumerator Flash()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = hitColor;

        yield return new WaitForSeconds(flashTime);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = originalColors[i];
    }

    private void Die()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        OnDied?.Invoke(this);
    }

    public void OnGetFromPool()
    {
        currentHealth = startingHealth;
        RestoreOriginalColors();
        OnHealthChanged?.Invoke(currentHealth, startingHealth);
    }

    public void OnReturnFromPool()
    {
        OnDied = null;
    }

    private void RestoreOriginalColors()
    {
        if (originalColors == null) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }
}
