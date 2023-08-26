using System;
using UnityEngine;
using UnityEngine.Serialization;

public class WormController : MonoBehaviour
{
    private BuildingManager b;
    public int length;
    public GameObject segmentPrefab;

    public Vector2 mapCenter;
    private Vector2 _gridOrigin;
    private Vector2 _gridSize;
    public float gridSpacing = 10f;
    private Vector2[] grid;
    private float cameraRatio = 20f; //3.5f ratio to 70 units

    public float smoothSpeed = 20f;
    public Vector3 targetPos;
    private Vector3 _targetPos;
    public GameObject targetObject;
    public GameObject wormHead;
    public Rigidbody2D wormHeadRb;

    private float _elapsedTime;
    private Vector3 _startPosition;
    public AnimationCurve curve;
    
    public float buildingAttackProbability = 0.3f;
    
    private bool _isAttacking = false;
    public bool onCooldown = false;
    

    public void Start()
    {
        wormHeadRb = wormHead.GetComponent<Rigidbody2D>();
        b = BuildingManager.Instance;
        GetComponentInChildren<WormTail>().InitializeTail(length, segmentPrefab, this.gameObject);
        InitializeGrid();
        NewTarget();
    }

    void InitializeGrid()
    {
        _gridOrigin = new Vector2(mapCenter.x - (cameraRatio / 2) * 3.5f, mapCenter.y);
        _gridSize = new Vector2((int)Math.Ceiling((mapCenter.x - _gridOrigin.x) * 2 / gridSpacing),
            (int)Math.Ceiling((mapCenter.x - _gridOrigin.x) * 2 / (2 * gridSpacing)));

        //initialize the grid downwards
        grid = new Vector2[(int)((_gridSize.x) * (_gridSize.y))];
        for (int i = 0, y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++, i++)
            {
                grid[i] = new Vector2(_gridOrigin.x + x * gridSpacing, _gridOrigin.y - y * gridSpacing);
            }
        }
    }
    void Update()
    {
        if (!onCooldown && _targetPos != Vector3.zero)
        {
            ComputePosition();
        }
        
        if (_isAttacking)
        {
            if (Vector2.Distance(wormHead.transform.position, targetPos) < 1f)
            {
                _targetPos = Vector3.zero;
                targetPos = Vector3.zero;
                wormHeadRb.gravityScale = 0.5f;
                wormHeadRb.drag = 0;
                Invoke("ResetAttackState", 4f);
                _isAttacking = false;
                onCooldown = true;
                return;
            }
        }
        
        if(Vector2.Distance(wormHead.transform.position, targetPos) < 1f)
        {
            NewTarget();
        }
    }

    void ResetAttackState()
    {
        onCooldown = false;
        wormHeadRb.gravityScale = 0;
        wormHeadRb.drag = 1.7f;
        NewTarget();
    }

    void ComputePosition()
    {
        _targetPos = Vector2.MoveTowards(_targetPos, targetPos, smoothSpeed * Time.deltaTime);
        targetObject.transform.position = _targetPos;
        
        _elapsedTime += Time.deltaTime;
        float percentageComplete = _elapsedTime / smoothSpeed;
        _targetPos = Vector3.Lerp(_startPosition, targetPos, curve.Evaluate(percentageComplete));
    }

    void NewTarget()
    {
        // probability to attack a building
        float buildRand = UnityEngine.Random.Range(0f, 1f);
        if (b.builtBuildings.Count > 0 && buildRand < buildingAttackProbability)
        {
            int buildRand2 = UnityEngine.Random.Range(0, b.builtBuildings.Count);
            targetPos = b.builtBuildings[buildRand2].transform.position;
            _targetPos = wormHead.transform.position;
            _startPosition = _targetPos;
            _elapsedTime = 0;
            _isAttacking = true;
            
            return;
        }
        
        // select a random point on the grid
        int rand = UnityEngine.Random.Range(0, grid.Length);
        targetPos = grid[rand];
        _targetPos = wormHead.transform.position;
        _startPosition = _targetPos;
        _elapsedTime = 0;
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

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_targetPos, 0.3f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPos, 0.3f);
    }
}
