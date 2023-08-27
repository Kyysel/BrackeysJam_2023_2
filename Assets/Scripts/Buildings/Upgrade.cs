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
    
    public Upgrade Perform(List<int> costArray)
    {
        costsArray = costArray;
        Initialize();
        foreach (string resource in resourceManager.resourceTypes)
        {
            resourceManager.resourceDict[resource] -= costsDict[resource];
        }
        HUDController.Instance.UpdateResourceHUD();
        Debug.Log("Building " + upgradeName);
        return this;
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
