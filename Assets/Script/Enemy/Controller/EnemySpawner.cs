using System;
using UnityEngine;

[Serializable]
public class SpawnParameter
{
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
    [SerializeField]
    private EnemyMovementType _moveType = EnemyMovementType.None;

    public float FirstSpawnInterval => _firstSpawnInterval;
    public float SpawnInterval => _spawnInterval;
    public int SpawnCount => _spawnCount;
    public EnemyMovementType MoveType => _moveType;
}

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("生成するEnemyPrefab")]
    [SerializeField]
    private GameObject _enemyPrefab = default;
    [Tooltip("生成位置")]
    [SerializeField]
    private Transform _spawnMuzzle = default;
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

    private float _spawnTimer = 0f;
    /// <summary> 初回生成を行ったかどうか </summary>
    private bool _isFirstSpawning = false;

    protected Vector2 SpawnPos => _spawnMuzzle.position;

    protected float SpawnInterval => _isFirstSpawning ? _spawnParam.SpawnInterval : _spawnParam.FirstSpawnInterval;

    /// <summary> 計測中 </summary>
    protected bool IsMeasuring => _spawnTimer <= SpawnInterval;

    public void Initialize()
    {
        _spawnMuzzle ??= transform;
    }

    public void Measuring(float deltaTime)
    {
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

        var enemy = EnemyManager.Instance.ObjectPool.SpawnObject(_enemyPrefab);
        enemy.transform.position = SpawnPos;

        var enemySystem = enemy.GetComponent<EnemyController>();
        enemySystem.MovementType = _spawnParam.MoveType;
        EnemyManager.Instance.EnemyMasterSystem.AddEnemy(enemySystem.EnemySystem);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_spawnInScreen) { return; }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, _spawnSearchRadius);
    }
#endif
}
