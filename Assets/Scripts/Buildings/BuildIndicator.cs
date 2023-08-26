using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildIndicator : MonoBehaviour
{
    public Color validColor;
    public Color invalidColor;
    public bool valid = true;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Building"))
        {
            GetComponent<SpriteRenderer>().color = invalidColor;
            valid = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Building"))
        {
            GetComponent<SpriteRenderer>().color = validColor;
            valid = true;
        }
    }
}
