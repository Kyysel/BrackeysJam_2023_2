using UnityEngine;

/* Credits to Blackthornprod for the original script*/
public class SegmentRotation : MonoBehaviour
{
    public float rotationSpeed;
    
    private Vector2 direction;
    public Transform target;

    void Update()
    {
        direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
