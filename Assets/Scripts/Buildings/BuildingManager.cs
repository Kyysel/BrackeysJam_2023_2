using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    private GameObject player;
    
    public List<Building> buildingsAvailable;
    public ResourceManager resourceManager;
    public List<Building> builtBuildings;

    public float gridWidth = 4.5f;
    public GameObject buildIndicator;
    
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

        InitializeGrid();
        // expensive but who cares ==> gamejam
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    
    void Update()
    {
        if (HUDController.Instance.hudActive)
        {
            buildIndicator.SetActive(true);
            float x = player.transform.position.x;
            float y = x - x%gridWidth + gridWidth/2;
            buildIndicator.transform.position = new Vector3(y, 0.5f);
        } else {
            buildIndicator.SetActive(false);
        }
    }

    void InitializeGrid()
    {
        
    }

    public void Build(Building building)
    {
        builtBuildings.Add(building.Build());
        float x = player.transform.position.x;
        float y = x - x%gridWidth + gridWidth/2;
        Instantiate(building.gameObject, new Vector3(y, 0), quaternion.identity);
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
