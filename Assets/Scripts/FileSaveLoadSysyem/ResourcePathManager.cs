using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePathManager : MonoBehaviourSingletonPersistent<ResourcePathManager>
{
    [SerializeField] private const string PathPassiveUpgradeData="Passive Upgrade Data";
    [SerializeField] private const string PathPlayerSO="Player SO";

    public string GetPassiveUpgradeDataPath()
    {
        return PathPassiveUpgradeData;
    }
    
    public string GetPlayerSOPath()
    {
        return PathPlayerSO;
    }
}
