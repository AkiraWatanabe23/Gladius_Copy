using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BulletHolder
{
    [field: SerializeField]
    public GameObject DefaultBullet { get; private set; }
    [field: SerializeField]
    public GameObject PenetrationBullet { get; private set; }
    [field: SerializeField]
    public GameObject MissileBullet { get; private set; }
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
    public List<Assault> AssaultEnemies { get; set; }
    public List<Shot> ShotEnemies { get; set; }
    public List<Boss> BossEnemies { get; set; }
    public BulletHolder BulletHolder => _bulletHolder;

    public EnemySpawner[] EnemySpawners { get => _enemySpawners; set => _enemySpawners = value; }

    public ObjectPool ObjectPool { get; set; }
}
