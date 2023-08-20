using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/* Credits to Blackthornprod for the original script*/
public class WormTail : MonoBehaviour
{
    private int length;
    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    public Vector3[] segmentPositions;
    public Vector3[] segmentVelocities;
    
    public Transform tailEnd;
    public GameObject[] bodyParts;
    

    public void InitializeTail(int length, GameObject segmentPrefab, GameObject parent)
    {
        this.length = length;
        segmentVelocities = new Vector3[length+1];
        segmentPositions = new Vector3[length+1];
        bodyParts = new GameObject[length+1];

        // First body part is the head
        bodyParts[0] = gameObject.transform.parent.gameObject;
        for (int i=1; i<length; i++)
        {
            GameObject segment = Instantiate(segmentPrefab, transform.position, Quaternion.identity);
            segment.transform.SetParent(parent.transform, false);
            bodyParts[i] = segment;
            // Following body parts are the segments
            segment.GetComponent<SegmentRotation>().target = bodyParts[i - 1].transform;
        }
        bodyParts[length] = tailEnd.gameObject;
        tailEnd.GetComponent<SegmentRotation>().target = bodyParts[length - 1].transform;
        
        ResetPos();
    }
    
    void Update()
    {
        // first segment follows the target (head)
        segmentPositions[0] = targetDir.position;
        
        // for every body part
        for (int i = 1; i <= length; i++)
        {
            Vector3 targetPos = segmentPositions[i - 1] + (segmentPositions[i] - segmentPositions[i - 1]).normalized * targetDist;
            segmentPositions[i] = Vector3.SmoothDamp(segmentPositions[i], targetPos, ref segmentVelocities[i], smoothSpeed);
            bodyParts[i].transform.position = segmentPositions[i];
        }

        tailEnd.position = segmentPositions[length];
    }

    private void ResetPos()
    {
        segmentPositions[0] = targetDir.position;
        for (int i = 1; i < length; i++)
        {
            segmentPositions[i] = segmentPositions[i - 1] + targetDir.right * targetDist;
        }
        tailEnd.position = segmentPositions[length];
    }
}
