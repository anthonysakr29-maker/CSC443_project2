using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ShopManager : MonoBehaviour
{
    [Header("Shop UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Turret Purchase")]
    [SerializeField] private GameObject turretToActivate;
    [SerializeField] private int turretCost = 500;

    [SerializeField] private int fireRateUpgradeCost = 300;
    [SerializeField] private int damageUpgradeCost = 300;

    private bool fireRateUpgraded;
    private bool damageUpgraded;

    private bool turretBought;
    private PlayerInput playerInput;

    private void Start()
    {
        shopPanel.SetActive(false);

        if (turretToActivate != null)
            turretToActivate.SetActive(false);

        playerInput = FindAnyObjectByType<PlayerInput>();
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetMessage("Shop opened");

        if (playerInput != null)
            playerInput.enabled = false;
    }

    public void BuyTurret()
    {
        if (turretBought)
        {
            SetMessage("Turret already bought");
            return;
        }

        if (GameManager.Instance.money < turretCost)
        {
            SetMessage("Not enough money");
            return;
        }

        GameManager.Instance.SpendMoney(turretCost);

        turretToActivate.SetActive(true);
        turretBought = true;

        SetMessage("Turret activated");
    }

    public void ContinueGame()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
        if (spawner != null)
            spawner.ContinueToNextWave();

        if (playerInput != null)
            playerInput.enabled = true;
    }

    private void SetMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
    }

    public void UpgradeFireRate()
    {
        if (!turretBought)
        {
            SetMessage("Buy turret first");
            return;
        }

        if (fireRateUpgraded)
        {
            SetMessage("Already upgraded");
            return;
        }

        if (!GameManager.Instance.SpendMoney(fireRateUpgradeCost))
        {
            SetMessage("Not enough money");
            return;
        }

        Turret turret = turretToActivate.GetComponent<Turret>();
        turret.UpgradeFireRate(1f);

        fireRateUpgraded = true;
        SetMessage("Fire rate upgraded");
    }

    public void UpgradeDamage()
    {
        if (!turretBought)
        {
            SetMessage("Buy turret first");
            return;
        }

        if (damageUpgraded)
        {
            SetMessage("Already upgraded");
            return;
        }

        if (!GameManager.Instance.SpendMoney(damageUpgradeCost))
        {
            SetMessage("Not enough money");
            return;
        }

        Turret turret = turretToActivate.GetComponent<Turret>();
        turret.UpgradeDamage(1);

        damageUpgraded = true;
        SetMessage("Damage upgraded");
    }
}