using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private ResourceManager resourceManager;
    public Dictionary<string, int> costsDict;
    public List<int> costsArray;
    public string name;
    public Sprite icon;
    
    public Building Build()
    {
        foreach (string resource in resourceManager.resourceTypes)
        {
            resourceManager.resourceDict[resource] -= costsDict[resource];
        }
        HUDController.Instance.UpdateResourceHUD();
        Debug.Log("Building...");
        return this;
    }

    public void Initialize()
    {
        costsDict = new Dictionary<string, int>();
        resourceManager = ResourceManager.Instance;
        
        for (int i=0; i < resourceManager.resourceTypes.Length; i++)
        {
            costsDict.Add(resourceManager.resourceTypes[i], costsArray[i]);
        }
    }
}
