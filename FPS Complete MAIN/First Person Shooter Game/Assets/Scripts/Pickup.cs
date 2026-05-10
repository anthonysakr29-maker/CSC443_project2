using System.Collections;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Respawn")]
    [SerializeField] private bool respawns;
    [SerializeField] private float respawnDelay = 20f;
    [SerializeField] private GameObject visualRoot;
    [SerializeField] private Collider pickupCollider;

    private bool available = true;

    private void Awake()
    {
        if (pickupCollider == null)
            pickupCollider = GetComponent<Collider>();

        if (visualRoot == null)
            visualRoot = gameObject;
    }

    private void Update()
    {
        if (available)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!available) return;
        if (!other.CompareTag("Player")) return;

        if (OnPickedUp(other))
        {
            if (respawns)
                StartCoroutine(RespawnRoutine());
            else
                Destroy(gameObject);
        }
    }

    private IEnumerator RespawnRoutine()
    {
        available = false;

        if (pickupCollider != null)
            pickupCollider.enabled = false;

        if (visualRoot != null)
            visualRoot.SetActive(false);

        yield return new WaitForSeconds(respawnDelay);

        if (visualRoot != null)
            visualRoot.SetActive(true);

        if (pickupCollider != null)
            pickupCollider.enabled = true;

        available = true;
    }

    protected abstract bool OnPickedUp(Collider player);
}