using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Shop UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TextMeshProUGUI messageText;

    [SerializeField] private Button buyTurretButton;
    [SerializeField] private TextMeshProUGUI buyTurretButtonText;

    [SerializeField] private Button fireRateButton;
    [SerializeField] private TextMeshProUGUI fireRateButtonText;

    [SerializeField] private Button damageButton;
    [SerializeField] private TextMeshProUGUI damageButtonText;

    [Header("Turret Purchase")]
    [SerializeField] private GameObject turretToActivate;
    [SerializeField] private int turretCost = 500;

    [SerializeField] private int fireRateUpgradeCost = 300;
    [SerializeField] private int damageUpgradeCost = 300;

    [Header("Consumables")]
    [SerializeField] private int fullHealCost = 250;
    [SerializeField] private int ammoRefillCost = 200;

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
        CursorLock.CanLockCursor = false;

        shopPanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerInput != null)
            playerInput.enabled = false;

        UpdateShopButtons();

        SetMessage("Shop opened");
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

        UpdateShopButtons();
        SetMessage("Turret activated");
    }

    public void ContinueGame()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f;

        CursorLock.CanLockCursor = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInput != null)
            playerInput.enabled = true;

        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
        if (spawner != null)
            spawner.ContinueToNextWave();
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
        UpdateShopButtons();
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
        UpdateShopButtons();
        SetMessage("Damage upgraded");
    }

    public void BuyFullHeal()
    {
        PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            SetMessage("Player health not found");
            return;
        }

        if (playerHealth.IsFullHealth())
        {
            SetMessage("Health already full");
            return;
        }

        if (!GameManager.Instance.SpendMoney(fullHealCost))
        {
            SetMessage("Not enough money");
            return;
        }

        playerHealth.Heal(999);
        SetMessage("Health restored");
    }

    public void BuyAmmoRefill()
    {
        WeaponSwitcher switcher = FindAnyObjectByType<WeaponSwitcher>();

        if (switcher == null || !switcher.HasUnlockedWeapons())
        {
            SetMessage("No weapons unlocked");
            return;
        }

        if (!GameManager.Instance.SpendMoney(ammoRefillCost))
        {
            SetMessage("Not enough money");
            return;
        }

        switcher.RefillAllUnlockedWeapons();
        SetMessage("Ammo refilled");
    }

    private void UpdateShopButtons()
    {
        if (buyTurretButton != null)
            buyTurretButton.interactable = !turretBought;

        if (buyTurretButtonText != null)
            buyTurretButtonText.text = turretBought ? "Turret Purchased" : "Buy Turret - $" + turretCost;

        if (fireRateButton != null)
            fireRateButton.interactable = turretBought && !fireRateUpgraded;

        if (fireRateButtonText != null)
            fireRateButtonText.text = fireRateUpgraded ? "Fire Rate Maxed" : "Upgrade Fire Rate - $" + fireRateUpgradeCost;

        if (damageButton != null)
            damageButton.interactable = turretBought && !damageUpgraded;

        if (damageButtonText != null)
            damageButtonText.text = damageUpgraded ? "Damage Maxed" : "Upgrade Damage - $" + damageUpgradeCost;
    }
}