using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance;

    [HideInInspector] public int stoneAmt, steelAmt, goldAmt, wormoniumAmt;

    public static event Action<ResourceType> CollectResourcesEvent;

    private void Awake()
    {

        #region set instance
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        #endregion

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeResource(ResourceType.STONE, 5);
    }

    public void ChangeResource(ResourceType type, int amount)
    {

        switch (type)
        {

            case ResourceType.STONE:
                stoneAmt += amount;
                break;

            case ResourceType.STEEL:
                steelAmt += amount;
                break;

            case ResourceType.GOLD:
                goldAmt += amount;
                break;

            case ResourceType.WORMONIUM:
                wormoniumAmt += amount;
                break;
        }

        if (CollectResourcesEvent != null)
            CollectResourcesEvent(type);

    }


}

public enum ResourceType
{
    STONE,
    STEEL,
    GOLD,
    WORMONIUM,
}
