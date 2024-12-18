using System.Collections.Generic;

[System.Serializable]
public class UpgradeData
{
    public List<Upgrade> Upgrades;

    public UpgradeData()
    {
        Upgrades = new List<Upgrade>();
    }
}