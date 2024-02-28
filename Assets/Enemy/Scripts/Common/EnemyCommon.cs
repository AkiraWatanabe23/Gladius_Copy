using System;
using UnityEngine;

[Serializable]
public class BulletHolder
{
    [field: SerializeField]
    public GameObject DefaultBullet { get; private set; }
}

[Serializable]
public class EnemyCommon
{
    [SerializeField]
    private Transform _player = default;
    [SerializeField]
    private GameObject _enemyCorePrefab = default;
    [SerializeField]
    private EnemySpawner[] _enemySpawners = default;
    [SerializeField]
    private BulletHolder _bulletHolder = new();

    public Transform Player => _player;
    public GameObject EnemyCorePrefab => _enemyCorePrefab;
    public BulletHolder BulletHolder => _bulletHolder;

    public EnemySpawner[] EnemySpawners { get => _enemySpawners; set => _enemySpawners = value; }

    public ObjectPool ObjectPool { get; set; }
}
