using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDeposit : MonoBehaviour
{

    public string resourceName;
    public int maxAmount;
    public int currentAmount;
    public float minDepth;
    public float maxDepth;
    public float minSize;
    public float maxSize;
    public float probability;
    
    void Start()
    {
        currentAmount = maxAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
