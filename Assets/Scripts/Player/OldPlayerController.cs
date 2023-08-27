using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class OldPlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    public float moveSpeed = 5f;
    
    private bool canMove = true;
    private bool canShoot = true;

    public Transform grapplePoint;
    public float grappleSpeed = 10f;
    public LineRenderer grappleLineRenderer;
    public GameObject grappleHead;
    private Rigidbody2D _grappleHeadRb;
    private bool _isGrappling = false;
    private bool _isRecallingGrapple = false;
    public Sprite GrappleOpenSprite;
    public Sprite GrappleClosedSprite;
    public SpriteRenderer grappleHeadSpriteRenderer;

    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _grappleHeadRb = grappleHead.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canShoot)
        {
            grappleHead.transform.position = grapplePoint.position;
        }
    }
    
    void FixedUpdate()
    {
        UpdateGrappleLine();
        
        Vector2 grappleHeadPosition = _grappleHeadRb.position;
        if (canMove)
        {
            _rb.MovePosition(_rb.position + moveSpeed * Time.fixedDeltaTime * _moveInput);
        }

        if (_isGrappling)
        {
            _grappleHeadRb.MovePosition(grappleHeadPosition + grappleSpeed * Time.fixedDeltaTime * Vector2.down);
        }
        if (_isRecallingGrapple)
        {
            Vector2 grapplePointPosition = new Vector2(grapplePoint.position.x, grapplePoint.position.y);
            _grappleHeadRb.MovePosition(grappleHeadPosition + 2f * grappleSpeed * Time.fixedDeltaTime * 
                (grapplePointPosition - grappleHeadPosition).normalized);
            if(Vector2.Distance(grapplePointPosition, grappleHeadPosition) < 0.5f)
            {
                grappleLineRenderer.enabled = false;
                _isRecallingGrapple = false;
                canMove = true;
                canShoot = true;
            }
        }
    }

    private void UpdateGrappleLine()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = grapplePoint.position;
        positions[1] = grappleHead.transform.position;
        grappleLineRenderer.SetPositions(positions);
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
    
    void OnShoot(InputValue value)
    {
        if (canShoot)
        {
            ShootGrapple();
            canShoot = false;
            canMove = false;
        }
        else
        {
            RecallGrapple();
        }
    }

    void ShootGrapple()
    {
        grappleLineRenderer.enabled = true;
        _isGrappling = true;
        grappleHeadSpriteRenderer.sprite = GrappleOpenSprite;
    }

    public void RecallGrapple()
    {
        _isGrappling = false;
        _isRecallingGrapple = true;
        grappleHeadSpriteRenderer.sprite = GrappleClosedSprite;
    }
    
}
