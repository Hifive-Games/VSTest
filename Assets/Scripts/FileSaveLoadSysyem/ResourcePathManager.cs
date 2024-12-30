using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePathManager : MonoBehaviourSingletonPersistent<ResourcePathManager>
{
    [SerializeField] private const string PathActiveUpgradeData="Active Upgrade Data";
    [SerializeField] private const string PathPassiveUpgradeData="Passive Upgrade Data";
    [SerializeField] private const string PathHeroBaseSO="Hero SO";

    public string GetActiveUpgradeDataPath()
    {
        return PathActiveUpgradeData;
    }
    public string GetPassiveUpgradeDataPath()
    {
        return PathPassiveUpgradeData;
    }
    
    public string GetHeroSOPath()
    {
        return PathHeroBaseSO;
    }
}
