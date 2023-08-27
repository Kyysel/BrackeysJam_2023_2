using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MechaWorm Properties")]
    public int length;
    public GameObject segmentPrefab;
    public int damage = 1;
    
    [Header("Player Stats")]
    public int collectAmount;
    
    void Start()
    {
        GetComponentInChildren<WormTail>().InitializeTail(length, segmentPrefab, this.gameObject);
    }
    
    
    
}
