using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    public static LevelUpPanel Instance { get; private set; }

    public GameObject UpgradePrefab;

    public List<UpgradeSO> allUpgrades;
    UpgradeSO[] choosenUpgrades;
    public int numberOfUpgrades = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void OnDisable()
    {
        ClearchoosenUpgrades();
    }

    void OnEnable()
    {
        RefreshUpgrades();
    }
    public void RefreshUpgrades()
    {
        if(allUpgrades == null)
        {
            allUpgrades = UpgradeHandler.Instance.GetAvailableUpgrades(new PlayerU());
        }

        allUpgrades.Clear();

        allUpgrades = UpgradeHandler.Instance.GetAvailableUpgrades(new PlayerU());
    }

    public void LevelUp()
    {
        gameObject.SetActive(true);
        choosenUpgrades = new UpgradeSO[3];
        for (int i = 0; i < choosenUpgrades.Length; i++)
        {
            UpgradeSO newUpgrade = allUpgrades[Random.Range(0, allUpgrades.Count)];
            while (CheckIfUpgradeIsAlreadySelected(newUpgrade))
            {
                newUpgrade = allUpgrades[Random.Range(0, allUpgrades.Count)];
            }
            choosenUpgrades[i] = newUpgrade;
            GameObject newUpgradeObject = Instantiate(UpgradePrefab, transform);
            newUpgradeObject.GetComponent<UpgradeUI>().SetUpgrade(newUpgrade);
        }
    }

    private bool CheckIfUpgradeIsAlreadySelected(UpgradeSO upgrade)
    {
        for (int i = 0; i < choosenUpgrades.Length; i++)
        {
            if (choosenUpgrades[i] == upgrade)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearchoosenUpgrades()
    {
        for (int i = 1; i < InterfaceManager.Instance.levelUpUI.transform.childCount; i++)
        {
            Destroy(InterfaceManager.Instance.levelUpUI.transform.GetChild(i).gameObject);
        }
    }

    public void AddUpgrade(UpgradeSO upgrade)
    {
        UpgradeHandler.Instance.AddUpgrade(upgrade);
    }

    public void RemoveUpgrade(UpgradeSO upgrade)
    {
        UpgradeHandler.Instance.RemoveUpgrade(upgrade);
    }

    public List<UpgradeSO> GetAvailableUpgrades(PlayerU player)
    {
        return UpgradeHandler.Instance.GetAvailableUpgrades(player);
    }

    public void SetCharacter(PlayerU player, Character character)
    {
        player.SetCharacter(character);
    }

    public void SetWeapon(PlayerU player, Weapon weapon)
    {
        player.SetWeapon(weapon);
    }

    public void AddUpgradeToWeapon(Weapon weapon, UpgradeSO upgrade)
    {
        weapon.availableUpgrades.Add(upgrade.upgradeName);
    }

    public void AddUpgradeToCharacter(Character character, UpgradeSO upgrade)
    {
        character.availableUpgrades.Add(upgrade.upgradeName);
    }

    public void RemoveUpgradeFromWeapon(Weapon weapon, UpgradeSO upgrade)
    {
        weapon.availableUpgrades.Remove(upgrade.upgradeName);
    }

    public void RemoveUpgradeFromCharacter(Character character, UpgradeSO upgrade)
    {
        character.availableUpgrades.Remove(upgrade.upgradeName);
    }
}
