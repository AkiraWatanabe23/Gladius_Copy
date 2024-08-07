﻿using UnityEngine;

/// <summary> レーザー（貫通） </summary>
public class LaserBullet : IBulletData
{
    [Tooltip("貫通数")]
    [SerializeField]
    private int _penetrationCount = 5;

    private int _initialPenetrationCount = -1;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public int GunnerLayer { get; set; }
    public Vector2 MoveForward { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        Rb2d.velocity = MoveForward * Speed;
    }

    public void Hit(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

        damageTarget.ReceiveDamage(AttackValue);
        if (_initialPenetrationCount < 0) { _initialPenetrationCount = _penetrationCount; }

        _penetrationCount--;

        if (_penetrationCount <= 0)
        {
            _penetrationCount = _initialPenetrationCount;
            GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
        }
    }
}
