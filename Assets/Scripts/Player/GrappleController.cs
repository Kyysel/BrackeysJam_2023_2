using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    public PlayerController player;
    public int collectAmount;
    void OnTriggerEnter2D(Collider2D other)
    {
        // if it collides with a resourceDeposit, add the corresponding resource to resourceDict in ResourceManager
        if (other.CompareTag("ResourceDeposit"))
        {
            ResourceDeposit rd = other.GetComponent<ResourceDeposit>();
            ResourceManager.Instance.ChangeResource(rd.resourceName, collectAmount);
            rd.currentAmount -= collectAmount;
            if (rd.currentAmount <= 0)
            {
                Destroy(other.gameObject);
            }
            player.RecallGrapple();
        }
        if (other.CompareTag("Worm"))
        {
            WormController wc = other.GetComponentInParent<WormController>();
            ResourceManager.Instance.ChangeResource("wormonium", collectAmount);
            wc.TakeDamage();
            player.RecallGrapple();
        }
    }
}
