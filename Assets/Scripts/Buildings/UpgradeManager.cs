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
        // if (HUDController.Instance.hudActive && _buildIndicatorUI.valid)
        // {
        //     float value = inputValue.Get<float>();   
        //     if (value != 0)
        //     {
        //         if (value <= buildingsAvailable.Count && canUpgrade(buildingsAvailable[(int)value - 1]))
        //         {
        //             Upgrade(buildingsAvailable[(int)value-1]);
        //         }
        //     }
        // }
    }
}
