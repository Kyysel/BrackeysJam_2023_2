using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorBuilding : Building
{
    public LineRenderer lineRenderer;
    public Transform laserTarget;
    void Start()
    {
        // call the UpdateTarget function every 0.2 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    

    void UpdateTarget()
    {
        
    }
}
