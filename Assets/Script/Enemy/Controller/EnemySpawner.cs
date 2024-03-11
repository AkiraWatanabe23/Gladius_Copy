using System;
using UnityEngine;

[Serializable]
public class SpawnParameter
{
    [SerializeField]
    private SpawnEnemy _spawnEnemy = SpawnEnemy.None;
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

    public float FirstSpawnInterval => _firstSpawnInterval;
    public float SpawnInterval => _spawnInterval;
    public int SpawnCount => _spawnCount;
}

public enum SpawnEnemy
{
    None,
    Assault,
    Shot
}

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("生成するEnemyPrefab")]
    [SerializeField]
    private GameObject _enemyPrefab = default;
    [Tooltip("生成位置")]
    [SerializeField]
    private Transform _spawnMuzzle = default;

    [Tooltip("")]
    [SerializeField]
    private SpawnParameter _spawnParam = new();

    private float _spawnTimer = 0f;
    /// <summary> 初回生成を行ったかどうか </summary>
    private bool _isFirstSpawning = false;

    protected Vector2 SpawnPos => _spawnMuzzle.position;

    protected float SpawnInterval => _isFirstSpawning ? _spawnParam.SpawnInterval : _spawnParam.FirstSpawnInterval;

    /// <summary> 計測中 </summary>
    protected bool IsMeasuring => _spawnTimer <= SpawnInterval;

    public void Initialize()
    {
        if (_spawnMuzzle == null)
        {
            Debug.Log("生成位置の割り当てがなかったため、自オブジェクトを生成位置に設定します");
            _spawnMuzzle = transform;
        }
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

        var enemySystem = enemy.GetComponent<EnemyController>().EnemySystem;
        EnemyManager.Instance.EnemyMasterSystem.AddEnemy(enemySystem);
    }
}
