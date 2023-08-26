using System;
using UnityEngine;

public class WormController : MonoBehaviour
{
    public int length;
    public GameObject segmentPrefab;

    public Vector2 mapCenter;
    private Vector2 _gridOrigin;
    private Vector2 _gridSize;
    public float gridSpacing = 10f;
    private Vector2[] grid;
    private float cameraRatio = 20f; //3.5f ratio to 70 units
    
    public void Start()
    {
        GetComponentInChildren<WormTail>().InitializeTail(length, segmentPrefab, this.gameObject);
        InitializeGrid();
    }

    void InitializeGrid()
    {
        // align with the buildings
        _gridOrigin = new Vector2(mapCenter.x - (cameraRatio/2)*3.5f, _gridOrigin.y);
        _gridSize = new Vector2((int)Math.Ceiling((mapCenter.x - _gridOrigin.x) * 2 / gridSpacing), 
            (int)Math.Ceiling((mapCenter.x - _gridOrigin.x) * 2 / (2 * gridSpacing)));
        
        //initialize the grid downwards
        grid = new Vector2[(int)((_gridSize.x) * (_gridSize.y))];
        for (int i = 0, y = 0; y < _gridSize.y; y++)
        {
            for (int x=0; x < _gridSize.x; x++, i++)
            {
                grid[i] = new Vector2(_gridOrigin.x + x * gridSpacing, _gridOrigin.y - y * gridSpacing);
            }
        }
    }

    public void TakeDamage()
    {
        length -= 1;
        GetComponentInChildren<WormTail>().RemoveSegment();
        if (length < 2)
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnDrawGizmos()
    {
        // show the grid
        Gizmos.color = Color.yellow;
        for (int i = 0; i < grid.Length; i++)
        {
            Gizmos.DrawSphere(grid[i], 0.2f);
        }
    }
}
