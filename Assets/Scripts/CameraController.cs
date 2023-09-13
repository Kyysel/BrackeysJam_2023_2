using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed;
    public float deadZoneHeight;
    
    private float _elapsedTime = 0f;
    private float _percentageComplete = 1f;

    // Update is called once per frame
    void Update()
    {
        
        // slowly move vertically towards the player, but not horizontally
        if (_percentageComplete>=1)
        {
            if (Mathf.Abs(player.position.y - transform.position.y) > deadZoneHeight)
            {
                Vector3 desiredPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);
                transform.position =
                    Vector3.MoveTowards(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            }
        }
        else
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);
            _elapsedTime += Time.deltaTime;
            _percentageComplete = _elapsedTime / smoothSpeed;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _percentageComplete);
            transform.position = smoothedPosition;
        }
        
    }

}
