using System;
using UnityEngine;

public class WormController : MonoBehaviour
{
    public int length;
    public GameObject segmentPrefab;
    
    // initialize the worm by creating its segments, appending the tail at the end
    // add the segmentrotation script to each segment and set the target to the segment before it
    // 

    public void Start()
    {
        GetComponentInChildren<WormTail>().InitializeTail(length, segmentPrefab, this.gameObject);
    }
}