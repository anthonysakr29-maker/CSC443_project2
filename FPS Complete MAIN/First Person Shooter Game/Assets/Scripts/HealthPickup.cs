using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private int healAmount = 30;

    protected override bool OnPickedUp(Collider player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health == null) return false;
        if (health.IsFullHealth()) return false;

        health.Heal(healAmount);
        return true;
    }
}