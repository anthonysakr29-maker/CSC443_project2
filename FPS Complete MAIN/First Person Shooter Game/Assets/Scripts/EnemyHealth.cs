using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IPoolable
{
    [SerializeField] private int startingHealth = 3;
    

    [Header("Rewards")]
    [SerializeField] private int scoreReward = 100;
    [SerializeField] private int moneyReward = 100;

    private int currentHealth;
    public int ScoreReward => scoreReward;
    public int MoneyReward => moneyReward;

    public event Action<EnemyHealth> OnDied;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDied?.Invoke(this);
    }

    public void OnGetFromPool()
    {
        currentHealth = startingHealth;
    }

    public void OnReturnFromPool()
    {
        OnDied = null;
    }
}
