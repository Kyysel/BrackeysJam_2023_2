using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance;
    public string[] resourceTypes;
    public Sprite[] resourceImages;
    
    public Dictionary<string, int> resourceDict = new Dictionary<string, int>();

    public static event Action<string> CollectResourcesEvent;

    private void Awake()
    {
        #region set instance
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        #endregion
    }

    private void Start()
    {
        if (resourceTypes.Length != resourceImages.Length)
        {
            Debug.LogError("Resource types and images are not the same length!");
        }
        
        foreach (string s in resourceTypes)
        {
            resourceDict.Add(s, 0);
        }
    }

    private void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeResource("stone", 5);
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
}