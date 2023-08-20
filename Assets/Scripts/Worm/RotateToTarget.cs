using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* Credits to Blackthornprod for the original script*/
public class RotateToTarget : MonoBehaviour
{
    public float rotationSpeed;
    private Vector2 direction;
    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));
        
        direction = cursorPos - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, cursorPos, moveSpeed * Time.deltaTime);
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (Vector2.Distance(transform.position, cursorPos) > 0.05f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}
