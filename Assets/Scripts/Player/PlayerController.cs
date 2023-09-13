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
        //remove any reources that you spawned on
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 30);
        foreach (Collider2D collider in colliders)
        {

            print($"found {gameObject.name}");

            if (collider.CompareTag("ResourceDeposit"))
            {
                print($"destroying {gameObject.name}");
                Destroy(collider.gameObject);
            }
        }

        GetComponentInChildren<WormTail>().InitializeTail(length, segmentPrefab, this.gameObject);
    }
    
    
    
}
