using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WormController : MonoBehaviour
{
    [Header("Worm Properties")]
    public int length;
    public int resourceCollectAmount;
    public float growthRate = 0.1f;
    public GameObject segmentPrefab;
    public GameObject wormHead;
    private Rigidbody2D _wormHeadRb;
    public int damage = 1;
    private bool invulnerable;

    [Header("Grid")]
    public Vector2 mapCenter;
    private Vector2 _gridOrigin;
    private Vector2 _gridSize;
    [Range(1,10)] public float gridSpacing = 10f;
    private Vector2[] _grid;
    public float cameraRatio = 20f; //3.5f ratio to 70 units

    [Header("Movement Properties")]
    public float smoothSpeed = 20f;
    public Vector3 targetPos;
    public GameObject targetObject;
    public AnimationCurve curve;
    private Vector3 _targetPos;
    private float _elapsedTime;
    private Vector3 _startPosition;
    [Range(0,1)] public float buildingAttackProbability = 0.3f;
    private bool _isAttacking = false;
    [HideInInspector] public bool onCooldown = false;

    [Header("References")] public GameObject buildingTarget;

    public BasicSound moveSound, damageSound;

    private Dictionary<string, int> _resourceDict;
    private WormTail _wormTail;

    public void Start()
    {
        _wormHeadRb = wormHead.GetComponent<Rigidbody2D>();
        _wormTail = GetComponentInChildren<WormTail>();
        _wormTail.InitializeTail(length, segmentPrefab, this.gameObject);
        
        InitializeGrid();
        NewTarget();
        AudioManager.instance.PlaySoundBaseOnTarget(moveSound, wormHead.transform, false);
        
        _resourceDict = new Dictionary<string, int>();
        foreach (string s in ResourceManager.Instance.resourceTypes)
        {
            _resourceDict.Add(s, 0);
        }
    }

    public WormController(int length, int damage, float buildingAttackProbability)
    {
        this.length = length;
        this.damage = damage;
        this.buildingAttackProbability = buildingAttackProbability;
    }

    void InitializeGrid()
    {
        _gridOrigin = new Vector2(mapCenter.x - (cameraRatio / 2) * 3.5f, mapCenter.y);
        _gridSize = new Vector2((int)Math.Ceiling((mapCenter.x - _gridOrigin.x) * 2 / gridSpacing),
            (int)Math.Ceiling((mapCenter.x - _gridOrigin.x) * 2 / (2 * gridSpacing)));

        //initialize the grid downwards
        _grid = new Vector2[(int)((_gridSize.x) * (_gridSize.y))];
        for (int i = 0, y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++, i++)
            {
                _grid[i] = new Vector2(_gridOrigin.x + x * gridSpacing, _gridOrigin.y - y * gridSpacing);
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
                _wormHeadRb.gravityScale = 0.5f;
                _wormHeadRb.drag = 0;
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
        _wormHeadRb.gravityScale = 0;
        _wormHeadRb.drag = 1.7f;
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
        if (buildRand < buildingAttackProbability)
        {
            targetPos = buildingTarget.transform.position;
            _targetPos = wormHead.transform.position;
            _startPosition = _targetPos;
            _elapsedTime = 0;
            _isAttacking = true;
            
            return;
        }
        
        // select a random point on the grid
        int rand = UnityEngine.Random.Range(0, _grid.Length);
        targetPos = _grid[rand];
        _targetPos = wormHead.transform.position;
        _startPosition = _targetPos;
        _elapsedTime = 0;
    }

    public void TakeDamage(int damage)
    {
        if (invulnerable) return;

        length -= damage;
        GetComponentInChildren<WormTail>().RemoveSegment();

        if (length < 2)
        {
            // add all the resources it had to the resource manager
            foreach (KeyValuePair<string, int> entry in _resourceDict)
            {
                ResourceManager.Instance.ChangeResource(entry.Key, entry.Value);
            }

            AudioManager.instance.PlaySoundBaseAtPos(damageSound, transform.position, gameObject.name);
            StopAllCoroutines();
            Destroy(this.gameObject);
            return;

        } else
        {
            AudioManager.instance.PlaySoundBaseOnTarget(damageSound, wormHead.transform, false);
        }

        invulnerable = true;
        StartCoroutine(SetInvulnerability());
    }
    
    //to stop weird collisions dealling all of the damage
    private IEnumerator SetInvulnerability()
    {
        yield return new WaitForSeconds(0.75f);
        invulnerable = false;
    }

    public void CollectResource(string type, int amount)
    {
        _resourceDict[type] += amount;
        foreach (GameObject segment in _wormTail.bodyParts)
        {
            segment.transform.localScale += new Vector3(growthRate, growthRate, growthRate);
        }
        _wormTail.targetDist += growthRate;
    }

    private void OnDrawGizmosSelected()  //got a null reference error
    {
        // show the grid
        Gizmos.color = Color.yellow;
        for (int i = 0; i < _grid.Length; i++)
        {
            Gizmos.DrawSphere(_grid[i], 0.2f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_targetPos, 0.3f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPos, 0.3f);
    }
}
