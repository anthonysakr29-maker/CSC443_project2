using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class Robot : MonoBehaviour
{
    [Header("References")]
    private NavMeshAgent agent;
    private FirstPersonController player;
    private PlayerHealth playerHealth;
    private Renderer[] renderers;


    [Header("Attack")]
    [SerializeField] private int damage = 15;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float hitRange = 1.8f;
    [SerializeField] private float windUpTime = 0.6f;
    [SerializeField] private float chargeSpeed = 9f;
    [SerializeField] private float chargeDuration = 0.35f;
    [SerializeField] private float recoveryTime = 1f;
    [SerializeField] private float backstepDistance = 2f;
    [SerializeField] private float backstepDuration = 0.25f;

    [Header("Visual Cue")]
    [SerializeField] private Color attackColor = Color.red;

    private bool isAttacking;
    private Color[] originalColors;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        renderers = GetComponentsInChildren<Renderer>();
        SaveOriginalColors();
    }

    private void Start()
    {
        FindPlayer();
    }

    private void OnEnable()
    {
        isAttacking = false;

        if (agent != null)
        {
            agent.enabled = false;
            agent.enabled = true;
            agent.Warp(transform.position);
            agent.isStopped = false;
        }

        FindPlayer();
        RestoreOriginalColors();
    }

    private void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        if (isAttacking) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackRange)
        {
            StartCoroutine(ChargeAttack());
        }
        else if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
    }

    private IEnumerator ChargeAttack()
    {
        isAttacking = true;

        agent.isStopped = true;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        SetColor(attackColor);

        yield return new WaitForSeconds(windUpTime);

        float timer = 0f;
        bool hasHit = false;

        while (timer < chargeDuration)
        {
            transform.position += transform.forward * chargeSpeed * Time.deltaTime;

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (!hasHit && distance <= hitRange)
            {
                playerHealth.TakeDamage(damage);
                hasHit = true;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(Backstep());

        RestoreOriginalColors();

        yield return new WaitForSeconds(recoveryTime);

        agent.isStopped = false;
        isAttacking = false;
    }

    private IEnumerator Backstep()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition - transform.forward * backstepDistance;

        float timer = 0f;

        while (timer < backstepDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timer / backstepDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }

    private void FindPlayer()
    {
        player = FindAnyObjectByType<FirstPersonController>();

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void SaveOriginalColors()
    {
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    private void SetColor(Color color)
    {
        foreach (Renderer rend in renderers)
        {
            rend.material.color = color;
        }
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