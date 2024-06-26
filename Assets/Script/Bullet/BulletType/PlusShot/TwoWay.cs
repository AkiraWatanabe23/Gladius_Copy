﻿using UnityEngine;

public class TwoWay : IBulletData
{
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
        if (collision.gameObject.TryGetComponent(out IDamageable damageTarget))
        {
            damageTarget.ReceiveDamage(AttackValue);
            GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
        }
        else if (collision.gameObject.TryGetComponent(out Ground _))
        {
            GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
        }
    }
}

public enum Diagonal
{
    Up,
    Down
}
