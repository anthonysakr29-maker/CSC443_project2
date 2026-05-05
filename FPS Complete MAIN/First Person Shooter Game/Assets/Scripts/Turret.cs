using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Combat")]
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private int damage = 1;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireSound;

    [Header("Pool")]
    [SerializeField] private int poolSize = 10;

    private ObjectPool<Projectile> projectilePool;
    private float nextFireTime;

    private void Start()
    {
        projectilePool = new ObjectPool<Projectile>(projectilePrefab, transform, poolSize);
    }

    private void Update()
    {
        EnemyHealth target = FindClosestEnemy();

        if (target == null) return;

        TrackTarget(target.transform);

        if (Time.time >= nextFireTime)
        {
            Fire();
        }
    }

    private EnemyHealth FindClosestEnemy()
    {
        EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        EnemyHealth closestEnemy = null;
        float closestDistance = detectionRange;

        foreach (EnemyHealth enemy in enemies)
        {
            if (!enemy.gameObject.activeInHierarchy) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void TrackTarget(Transform target)
    {
        Vector3 direction = (target.position - head.position).normalized;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        head.rotation = Quaternion.Slerp(
            head.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void Fire()
    {
        nextFireTime = Time.time + (1f / fireRate);

        Projectile projectile = projectilePool.Get(firePoint.position, firePoint.rotation);
        projectile.Fire(damage, ReturnProjectile);

        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    private void ReturnProjectile(Projectile projectile)
    {
        projectilePool.Return(projectile);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void UpgradeFireRate(float amount)
    {
        fireRate += amount;
    }

    public void UpgradeDamage(int amount)
    {
        damage += amount;
    }
}