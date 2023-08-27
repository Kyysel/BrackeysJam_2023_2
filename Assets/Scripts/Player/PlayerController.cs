using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Worm Properties")]
    public int length;
    public GameObject segmentPrefab;
    public int damage = 1;
    
    void Start()
    {
        GetComponentInChildren<WormTail>().InitializeTail(length, segmentPrefab, this.gameObject);
    }
    
}
