using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Upgrade : MonoBehaviour
{
    private ResourceManager resourceManager;
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
        HUDController.Instance.UpdateResourceHUD();
        Debug.Log("Building " + upgradeName);
        
        //TODO update the costs
    }

    public void Initialize()
    {
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
