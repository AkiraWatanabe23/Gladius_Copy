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
    [field: SerializeField]
    public GameObject TrackingBullet { get; private set; }
    [field: SerializeField]
    public GameObject BombBullet { get; private set; }

    [SerializeField]
    private List<InitialBullet> _initialBullets = default;
    [SerializeField]
    private List<PlusShot> _plusShots = default;

    private Dictionary<InitialBulletType, GameObject> _bulletsDictionary = default;
    private Dictionary<PlusShotType, GameObject> _plusShotsDictionary = default;

    public Dictionary<InitialBulletType, GameObject> BulletsDictionary
    {
        get
        {
            if (_bulletsDictionary == null)
            {
                _bulletsDictionary = new();
                foreach (var bullet in _initialBullets) { _bulletsDictionary.Add(bullet.BulletType, bullet.BulletPrefab); }
            }
            return _bulletsDictionary;
        }
    }
    public Dictionary<PlusShotType, GameObject> PlusShotsDictionary
    {
        get
        {
            if (_plusShotsDictionary == null)
            {
                _plusShotsDictionary = new();
                foreach (var shot in _plusShots) { _plusShotsDictionary.Add(shot.ShotType, shot.BulletPrefab); }
            }
            return _plusShotsDictionary;
        }
    }
}

[Serializable]
public class InitialBullet
{
    [field: SerializeField]
    public InitialBulletType BulletType { get; private set; }
    [field: SerializeField]
    public GameObject BulletPrefab { get; private set; }
}

[Serializable]
public class PlusShot
{
    [field: SerializeField]
    public PlusShotType ShotType { get; private set; }
    [field: SerializeField]
    public GameObject BulletPrefab { get; private set; }
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
