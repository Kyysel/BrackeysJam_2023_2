using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Building : MonoBehaviour
{
    private ResourceManager resourceManager;
    public Dictionary<string, int> costsDict;
    public List<int> costsArray;
    [FormerlySerializedAs("name")] public string buildingName;
    public Sprite icon;
    
    
    public Building Build(List<int> costArray)
    {
        costsArray = costArray;
        Initialize();
        foreach (string resource in resourceManager.resourceTypes)
        {
            resourceManager.resourceDict[resource] -= costsDict[resource];
        }
        HUDController.Instance.UpdateResourceHUD();
        Debug.Log("Building " + buildingName);
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
