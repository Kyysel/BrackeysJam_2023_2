using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    
    public List<Building> buildingsAvailable;
    public ResourceManager resourceManager;
    public List<Building> builtBuildings;
    
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
        foreach (Building b in buildingsAvailable)
        {
            b.Initialize();
        }
    }

    
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F1))
        // {
        //     if (canBuild(buildingsAvailable[0]))
        //     {
        //         Build(buildingsAvailable[0]);
        //     }
        // }
    }

    public void Build(Building building)
    {
        builtBuildings.Add(building.Build());
    }

    public bool canBuild(Building building)
    {
        foreach (string resource in resourceManager.resourceDict.Keys)
        {
            if (resourceManager.resourceDict[resource] - building.costsDict[resource] < 0)
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
                if (value <= buildingsAvailable.Count && canBuild(buildingsAvailable[(int)value - 1]))
                {
                    Build(buildingsAvailable[(int)value-1]);
                }
            }
        }
    }
}
