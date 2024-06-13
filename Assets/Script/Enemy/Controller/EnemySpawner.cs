﻿using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SpawnParameter
{
    [Tooltip("Enemy生成時に付与する生成後の動き")]
    [SerializeField]
    private EnemyMovementType _moveType = EnemyMovementType.None;
    [Tooltip("初回生成間隔（sec）")]
    [SerializeField]
    private float _firstSpawnInterval = 1f;
    [Tooltip("2回目以降の生成間隔（sec）")]
    [SerializeField]
    private float _spawnInterval = 1f;
    [Min(1)]
    [Tooltip("一度に生成する数")]
    [SerializeField]
    private int _spawnCount = 1;
    [Min(1)]
    [Tooltip("このSpawnerが生成できる最大数")]
    [SerializeField]
    private int _maxSpawnCount = 5;
    [SerializeField]
    private PathDrawer _terrainPath = new();

    public EnemyMovementType MoveType => _moveType;
    public float FirstSpawnInterval => _firstSpawnInterval;
    public float SpawnInterval => _spawnInterval;
    public int SpawnCount => _spawnCount;
    public int MaxSpawnCount => _maxSpawnCount;
    public PathDrawer TerrainPath => _terrainPath;
}

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("画面内で生成するか")]
    [SerializeField]
    private bool _spawnInScreen = true;

    [Tooltip("Enemy生成に関するパラメータ")]
    [SerializeField]
    private SpawnParameter _spawnParam = new();

    [Header("Only In Screen")]
    [Tooltip("サーチ範囲（画面内生成の時のみ適応）")]
    [SerializeField]
    private float _spawnSearchRadius = 1f;

    [Header("Only Out Screen")]
    [Tooltip("カメラのどちら側のColliderに反応するか（画面外生成の時のみ適応）")]
    [SerializeField]
    private SpawnSearchDirection _spawnDir = SpawnSearchDirection.Left;

    private EnemyManager _enemyManager = default;
    private GameObject _enemyPrefab = default;
    private float _spawnTimer = 0f;
    /// <summary> 初回生成を行ったかどうか </summary>
    private bool _isFirstSpawning = false;
    private bool _isEnterArea = false;
    private Transform _cameraRightSide = default;
    private int _spawnCounter = 0;

    protected float SpawnInterval => _isFirstSpawning ? _spawnParam.SpawnInterval : _spawnParam.FirstSpawnInterval;

    /// <summary> 計測中 </summary>
    protected bool IsMeasuring => _spawnTimer <= SpawnInterval;

    public void Initialize(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
        _enemyPrefab = _enemyManager.EnemyPrefabsDict[_spawnParam.MoveType];
        _cameraRightSide = GameObject.Find("Main Camera").GetComponent<CameraController>().RightSpawnPoint;

        _spawnParam.TerrainPath.Initialize();
    }

    public void Measuring(float deltaTime)
    {
        if (!_spawnInScreen) { return; }

        if (!_isEnterArea)
        {
            if (_spawnParam.MoveType != EnemyMovementType.RightAngle)
            {
                var targetPos = _cameraRightSide.position;
                _isEnterArea = targetPos.x >= transform.position.x;
            }
            else
            {
                var targetPos = GameManager.Instance.PlayerTransform.position;
                var sqrDistance = (targetPos - transform.position).sqrMagnitude;
                if (sqrDistance <= _spawnSearchRadius) { _isEnterArea = true; }
            }
        }
        _spawnTimer += deltaTime;
        if (IsMeasuring) { return; }

        EnemySpawn();
        _spawnTimer = 0f;
    }

    /// <summary> Prefab生成 </summary>
    private void EnemySpawn()
    {
        if (_enemyPrefab == null) { Debug.LogError("生成するオブジェクトの割り当てがありません"); return; }

        if (!_isFirstSpawning) { _isFirstSpawning = true; }
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for (int i = 0; i < _spawnParam.SpawnCount; i++)
        {
            if (_spawnCounter >= _spawnParam.MaxSpawnCount) { yield break; }

            if (/*GameManager.Instance.EnemyAnnihilated != null &&*/ GameManager.Instance.EnemyAnnihilated.SpawnedEnemy(1))
            {
                var enemy = GameManager.Instance.ObjectPool.SpawnObject(_enemyPrefab);

                var spawnPos = _cameraRightSide.position;
                //位置調整用に1.5f
                spawnPos.x -= 1.5f;
                spawnPos.y = transform.position.y;
                enemy.transform.position = spawnPos;

                var enemySystem = enemy.GetComponent<EnemyController>();
                enemySystem.MovementType = _spawnParam.MoveType;
                if (enemySystem.MovementType == EnemyMovementType.FollowTerrain)
                {
                    var assault = (Assault)enemySystem.EnemySystem;
                    assault.MoveRoute = _spawnParam.TerrainPath.PathPoints;
                }
                enemySystem.Initialize();
                _enemyManager.AddEnemy(enemySystem.EnemySystem);
                _spawnCounter++;
                yield return new WaitForSeconds(0.3f);
            }
            else { yield break; }
        }
        yield return null;
        if (!_spawnInScreen) { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_spawnInScreen) { return; }

        if (collision.gameObject.TryGetComponent(out CameraCollider camera))
        {
            if (camera.ColliderDirection == _spawnDir) { EnemySpawn(); }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_spawnInScreen) { return; }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, _spawnSearchRadius / 2f);
    }
#endif
}

public enum EnemyMovementType
{
    None,
    Straight,
    RightAngle,
    FigureEight,
    ZShapedMeandering,
    FollowTerrain,
    CrawlGround,
    Jumping
}

public enum SpawnSearchDirection
{
    Left,
    Right,
}
