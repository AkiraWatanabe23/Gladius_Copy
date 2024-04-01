using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAttack : PlayerSystemBase
{
    [Tooltip("弾を射出するMuzzle")]
    [SerializeField]
    private List<Transform> _spawnMuzzles = default;
    [SerializeField]
    private int _attackValue = 1;
    [SerializeField]
    private float _attackInterval = 1f;
    [SerializeField]
    private LayerMask _playerLayer = default;
    [SerializeField]
    private PlusShotType _plusShotBullet = PlusShotType.None;
    [Tooltip("初期ショット")]
    [SerializeField]
    private List<InitialBulletType> _initialBullets = default;

    private int _bulletIndex = 0;
    private List<GameObject> _bullets = default;
    private GameObject _plusShot = default;

    public int AttackValue => _attackValue;
    public PlusShotType PlusShotBullet
    {
        get => _plusShotBullet;
        set
        {
            _plusShotBullet = value;
            _plusShot = GameManager.Instance.BulletHolder.PlusShotsDictionary[value];
        }
    }

    protected List<GameObject> Bullets
    {
        get
        {
            if (_bullets == null)
            {
                _bullets = new();
                foreach (var bullet in _initialBullets)
                {
                    _bullets.Add(GameManager.Instance.BulletHolder.BulletsDictionary[bullet]);
                }
            }
            return _bullets;
        }
    }
    protected bool IsGetShootInput => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    protected bool IsGetBulletChangeInputUp => Input.mouseScrollDelta.y >= 1f;
    protected bool IsGetBulletChangeInputDown => Input.mouseScrollDelta.y <= -1f;

    public override void OnUpdate()
    {
        if (IsGetShootInput) { Attack(); }
        if (IsGetBulletChangeInputUp) { BulletChange(1); }
        if (IsGetBulletChangeInputDown) { BulletChange(-1); }
    }

    private void Attack()
    {
        Debug.Log("attack");
        if (_spawnMuzzles == null || _spawnMuzzles.Count == 0) { return; }
        for (int i = 0; i < _spawnMuzzles.Count; i++)
        {
            GameObject bullet = null;
            if (i == 0)
            {
                bullet = GameManager.Instance.ObjectPool.SpawnObject(Bullets[_bulletIndex]);
            }
            else if (i < 0 && _plusShotBullet != PlusShotType.None)
            {
                //PlusShotを撃つ
                bullet = GameManager.Instance.ObjectPool.SpawnObject(_plusShot);
            }
            bullet.transform.position = _spawnMuzzles[i].position;
            var bulletData = bullet.GetComponent<BulletController>();
            bulletData.Initialize(1f, _attackValue, _playerLayer, Vector2.right);
        }
    }

    private void BulletChange(int changeValue = 1)
    {
        if (_bulletIndex + changeValue >= Bullets.Count) { _bulletIndex = 0; return; }
        if (_bulletIndex + changeValue < 0) { _bulletIndex = Bullets.Count - 1; return; }

        _bulletIndex += changeValue;
    }
}
