using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Upgrade : MonoBehaviour
{
    private ResourceManager resourceManager;
    private UpgradeManager upgradeManager;
    private int upgradeLevel = 0;
    public Dictionary<string, int> costsDict;
    public List<int> costsArray;
    public string upgradeName;
    public Sprite icon;
    
    public void Perform()
    {
        foreach (string resource in resourceManager.resourceTypes)
        {
            resourceManager.resourceDict[resource] -= costsDict[resource];
        }
        UpgradeManager.Instance.level++;
        upgradeLevel++;
        HUDController.Instance.UpdateResourceHUD();
        Debug.Log("Building " + upgradeName);
        
        UpgradeCosts();
        
    }

    //TODO implement function
    public void UpgradeCosts()
    {
        
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
    
    
}
