using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePathManager : MonoBehaviourSingletonPersistent<ResourcePathManager>
{
    [SerializeField] private const string PathPassiveUpgradeData="Passive Upgrade Data";

    public string GetPassiveUpgradeDataPath()
    {
        return PathPassiveUpgradeData;
    }
    

}
