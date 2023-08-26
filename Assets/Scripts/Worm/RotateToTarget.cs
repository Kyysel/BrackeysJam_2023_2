using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/* Credits to Blackthornprod for the original script*/
public class RotateToTarget : MonoBehaviour
{
    public float rotationSpeed;
    private Vector2 direction;
    public float moveSpeed;
    public float maxSpeed;
    private Rigidbody2D _rb;
    
    public GameObject target;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        direction = target.transform.position - transform.position;
        if (_rb.velocity.magnitude < maxSpeed && direction.magnitude > 1f)
        {
            _rb.AddForce(direction.normalized * moveSpeed);
        }
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
