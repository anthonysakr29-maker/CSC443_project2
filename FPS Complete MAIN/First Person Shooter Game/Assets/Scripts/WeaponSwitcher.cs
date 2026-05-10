using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private ActiveWeapon activeWeapon;

    private StarterAssetsInputs inputs;
    private Weapon[] allWeapons;
    private List<Weapon> unlockedWeapons = new List<Weapon>();
    private int currentIndex = -1;

    private void Awake()
    {
        inputs = GetComponentInParent<StarterAssetsInputs>();
        allWeapons = GetComponentsInChildren<Weapon>(true);
    }

    private void Start()
    {
        foreach (Weapon weapon in allWeapons)
        {
            weapon.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (unlockedWeapons.Count < 2) return;

        bool shouldSwitch = false;

        if (inputs != null && inputs.switchWeapon)
        {
            inputs.SwitchWeaponInput(false);
            shouldSwitch = true;
        }

        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            shouldSwitch = true;
        }

        if (Mouse.current != null && Mouse.current.scroll.ReadValue().y != 0)
        {
            shouldSwitch = true;
        }

        if (shouldSwitch)
        {
            int nextIndex = (currentIndex + 1) % unlockedWeapons.Count;
            EquipWeapon(nextIndex);
        }
    }

    public void UnlockWeapon(Weapon weapon)
    {
        if (weapon == null) return;

        if (!unlockedWeapons.Contains(weapon))
        {
            unlockedWeapons.Add(weapon);
        }

        EquipWeapon(unlockedWeapons.IndexOf(weapon));
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= unlockedWeapons.Count) return;

        foreach (Weapon weapon in unlockedWeapons)
        {
            weapon.gameObject.SetActive(false);
        }

        currentIndex = index;

        Weapon selectedWeapon = unlockedWeapons[currentIndex];
        selectedWeapon.gameObject.SetActive(true);

        activeWeapon.SwitchWeapon(selectedWeapon);

        Debug.Log("Equipped weapon: " + selectedWeapon.Data.weaponName);
    }

    public void RefillAllUnlockedWeapons()
    {
        foreach (Weapon weapon in unlockedWeapons)
        {
            weapon.RefillAmmo();
        }
    }

    public bool HasUnlockedWeapons()
    {
        return unlockedWeapons.Count > 0;
    }
}