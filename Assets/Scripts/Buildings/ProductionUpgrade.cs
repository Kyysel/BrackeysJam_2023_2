using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionUpgrade : Upgrade
{
    /* The Production upgrade will convert resources their refined version over time
    The production speed depends on the level of the upgrade
    */
    
    public float productionSpeed = 1f;
    
    private void Start()
    {
        StartProduction();
    }

    private void StartProduction()
    {
        StartCoroutine(TransformResource("stone", 1/productionSpeed));
        StartCoroutine(TransformResource("steel", 1/productionSpeed*1.5f));
        StartCoroutine(TransformResource("gold", 1/productionSpeed*2f));
        StartCoroutine(TransformResource("wormonium", 1/productionSpeed*2.5f));
    }
    
    private IEnumerator TransformResource(string resource, float productionSpeed)
    {
        WaitForSeconds waitTime = new WaitForSeconds(productionSpeed);
        Debug.Log("started production of " + resource);
        
        while (true)
        {
            if (resourceManager.resourceDict[resource] > 0)
            {
                resourceManager.ChangeResource(resource, -1);
                resourceManager.ChangeRefinedResource(resource, 1);
            }

            yield return waitTime;
        }
    }
    
    public override void UpdateStats()
    {
        productionSpeed += 1f;
        StopAllCoroutines();
        StartProduction();
    }
}
