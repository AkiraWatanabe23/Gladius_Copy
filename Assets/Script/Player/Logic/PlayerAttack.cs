using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAttack : PlayerSystemBase
{
    [SerializeField]
    private Transform _spawnMuzzle = default;
    [SerializeField]
    private int _attackValue = 1;
    [SerializeField]
    private float _attackInterval = 1f;
    [Tooltip("初期ショット")]
    [SerializeField]
    private List<InitialBulletType> _initialBullets = default;

    private int _bulletIndex = 0;
    private List<GameObject> _bullets = default;

    public List<GameObject> Bullets
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

    public int AttackValue => _attackValue;

    protected bool IsGetShootInput => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    protected bool IsGetBulletChangeInputUp => Input.GetKeyDown(KeyCode.UpArrow);
    protected bool IsGetBulletChangeInputDown => Input.GetKeyDown(KeyCode.DownArrow);

    public override void OnUpdate()
    {
        if (IsGetShootInput) { Attack(); }
        if (IsGetBulletChangeInputUp) { BulletChange(1); }
        if (IsGetBulletChangeInputDown) { BulletChange(-1); }
    }

    private void Attack()
    {
        Debug.Log("attack");
        //var bullet = GameManager.Instance.ObjectPool.SpawnObject(_bullets[_bulletIndex]);
        //bullet.transform.position = _spawnMuzzle.position;
    }

    private void BulletChange(int changeValue = 1)
    {
        if (_bulletIndex + changeValue >= _bullets.Count) { _bulletIndex = 0; return; }
        if (_bulletIndex + changeValue < 0) { _bulletIndex = _bullets.Count - 1; return; }

        _bulletIndex += changeValue;
    }
}
