using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    
    public ResourceManager resourceManager;
    
    public List<Upgrade> upgrades;
    
    public int level = 0;

    
    private void Awake()
    {
        #region set instance
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        #endregion
    }
    
    void Start()
    {
        resourceManager = ResourceManager.Instance;
        foreach (var upgrade in upgrades)
        {
            upgrade.Initialize();
        }
    }
    
    public bool canUpgrade(Upgrade upgrade)
    {
        foreach (string resource in resourceManager.resourceDict.Keys)
        {
            if (resourceManager.resourceDict[resource] - upgrade.costsDict[resource] < 0)
            {
                return false;
            }
        }
        return true;
    }

    void OnBuildingNumber(InputValue inputValue)
    {
        if (HUDController.Instance.hudActive)
        {
            float value = inputValue.Get<float>();   
            if (value != 0)
            {
                if (value <= upgrades.Count && canUpgrade(upgrades[(int)value - 1]))
                {
                    upgrades[(int)value-1].Perform();
                }
            }
        }
    }
}
