using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Upgrade : MonoBehaviour
{
    [HideInInspector] public ResourceManager resourceManager;
    private UpgradeManager upgradeManager;
    public string upgradeName;
    public Sprite icon;
    private int upgradeLevel = 0;
    public Dictionary<string, int> costsDict;
    [HideInInspector] public UpgradePanel upgradePanel;
    // used for initialization
    public List<int> costsArray;
    
    
    public void Perform()
    {
        foreach (string resource in resourceManager.resourceTypes)
        {
            resourceManager.refinedResourceDict[resource] -= costsDict[resource];
        }
        UpgradeManager.Instance.level++;
        upgradeLevel++;
        HUDController.Instance.UpdateResourceHUD();
        Debug.Log("Building " + upgradeName);
        
        UpgradeCosts();
        upgradeLevel++;
        UpdateStats();
    }

    /**
     * Upgrade costs by 30%
     */
    public void UpgradeCosts()
    {
        foreach (var resource in resourceManager.resourceTypes)
        {
            costsDict[resource] = (int)Math.Ceiling(costsDict[resource] * 1.3f);
        }
        UpdateHUD();
    }

    /**
     * Update the upgrade panel with the new costs
     * the resource order is the same as the one in the resource manager
     */
    public void UpdateHUD()
    {
        int i = 0;
        foreach (var resource in costsDict.Keys)
        {
            TextMeshProUGUI text = upgradePanel.resourcePanels[i].GetComponentInChildren<TextMeshProUGUI>();
            int cost = costsDict[resource];
            text.text = cost.ToString();
            i++;
        }
    }

    public void Initialize()
    {
        upgradeLevel = 0;
        upgradeManager = UpgradeManager.Instance;
        
        costsDict = new Dictionary<string, int>();
        resourceManager = ResourceManager.Instance;
        
        for (int i=0; i < resourceManager.resourceDict.Count; i++)
        {
            try 
            {
                costsDict.Add(resourceManager.resourceTypes[i], costsArray[i]);
            } catch (System.ArgumentException)
            {
                costsDict.Add(resourceManager.resourceTypes[i], 0);
            }
            
        }
    }

    public virtual void UpdateStats()
    {
        throw new NotImplementedException();
    }


}
