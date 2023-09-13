using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourceSpawnerManager : MonoBehaviour
{
    public Transform ground;
    public List<Sprite> resourceSprites;
    public List<ResourceDeposit> resourceDeposits;

    public float resourceDensity;

    public Vector2 gridOrigin;
    public Vector2 gridSize;
    public float gridSpacing=4.5f;
    public Vector2[] grid;
    void Start()
    {
        // align with the buildings
        gridOrigin = new Vector2(gridOrigin.x - (gridOrigin.x % gridSpacing) + gridSpacing/2, gridOrigin.y);
        
        //initialize the grid downwards
        grid = new Vector2[(int)((gridSize.x) * (gridSize.y))];
        for (int i = 0, y = 0; y < gridSize.y; y++)
        {
            for (int x=0; x < gridSize.x; x++, i++)
            {
                grid[i] = new Vector2(gridOrigin.x + x * gridSpacing, gridOrigin.y - y * gridSpacing);
            }
        }
        
         SpawnResources();
    }

    void SpawnResources()
    {
        int[] gridCopy = new int[grid.Length];
        foreach (ResourceDeposit resourceDeposit in resourceDeposits)
        {
            print("generation of " + resourceDeposit.resourceName);
            // go through each grid point and spawn a resource with probability x if its within the min and max depth
            // the size controls how many resources are spawned
            for (int i = 0; i < grid.Length; i++)
            {
                if(gridCopy[i] == 0 && -grid[i].y >= resourceDeposit.minDepth && -grid[i].y <= resourceDeposit.maxDepth)
                {
                    float rand = UnityEngine.Random.Range(0f, 1f);
                    float actualProbability = resourceDeposit.probability * resourceDensity;
                    if (rand < actualProbability)
                    {
                        // spawn a resource
                        Vector3 spawnLocation = grid[i] + new Vector2(UnityEngine.Random.Range(-1/2f, 1/2f), UnityEngine.Random.Range(-1f, 1f));
                        GameObject inst = Instantiate(resourceDeposit.gameObject, spawnLocation, Quaternion.identity);
                        inst.transform.SetParent(ground);
                        gridCopy[i] = 1;
                    }
                }
            }
        }
    }
}
