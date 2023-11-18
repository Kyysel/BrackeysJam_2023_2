using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance;
    public string[] resourceTypes;
    public Sprite[] resourceImages;
    public Sprite[] refinedResourceImages;
    public BasicSound resourceDestroySound;
    
    public Dictionary<string, int> resourceDict = new Dictionary<string, int>();
    public Dictionary<string, int> refinedResourceDict = new Dictionary<string, int>();

    public static event Action<string> CollectResourcesEvent;

    private void Awake()
    {
        #region set instance
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        #endregion
        
        if (resourceTypes.Length != resourceImages.Length)
        {
            Debug.LogError("Resource types and images are not the same length!");
        }
        
        foreach (string s in resourceTypes)
        {
            resourceDict.Add(s, 0);
        }

        refinedResourceDict = new Dictionary<string, int>(resourceDict);
    }

    private void Update()
    {
        // add resources for testing when pressing F1
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (string s in resourceTypes)
            { 
                ChangeResource(s, 100);
            }
        }
    }

    public void ChangeResource(string type, int amount)
    {
        resourceDict[type] += amount;

        if (CollectResourcesEvent != null)
        {
            CollectResourcesEvent(type);
        }
    }

    public void ChangeRefinedResource(string type, int amount)
    {
        refinedResourceDict[type] += amount;
        if (CollectResourcesEvent != null)
        {
            CollectResourcesEvent(type);
        }
    }
}
