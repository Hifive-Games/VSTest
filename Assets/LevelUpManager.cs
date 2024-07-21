using Unity.Transforms;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{

    public static LevelUpManager instance;
    public static LevelUpManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelUpManager>();
            }
            return instance;
        }
    }
    public LevelUpPool levelUpPool;

    public GameObject upgrade;
    
    public Upgrade[] choosenUpgrades;

    private void Start()
    {
        LevelUp();
    }

    public void LevelUp()
    {
        choosenUpgrades = new Upgrade[3];
        for (int i = 0; i < choosenUpgrades.Length; i++)
        {
            Upgrade newUpgrade = levelUpPool.GetRandomUpgrade();
            while (CheckIfUpgradeIsAlreadySelected(newUpgrade))
            {
                newUpgrade = levelUpPool.GetRandomUpgrade();
            }
            choosenUpgrades[i] = newUpgrade;
            choosenUpgrades[i].rarity = levelUpPool.GiveARandomRarity();
            GameObject newUpgradeObject = Instantiate(upgrade, InterfaceManager.Instance.levelUpUI.transform);
            newUpgradeObject.GetComponent<UpgradeUI>().SetUpgrade(newUpgrade);
        }
    }

    public void ClearchoosenUpgrades()
    {
        for (int i = 1; i < InterfaceManager.Instance.levelUpUI.transform.childCount; i++)
        {
            Destroy(InterfaceManager.Instance.levelUpUI.transform.GetChild(i).gameObject);
        }
    }

    private bool CheckIfUpgradeIsAlreadySelected(Upgrade upgrade)
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
}

