﻿using System.Linq;
using UnityEngine;

/// <summary> 追尾 </summary>
public class HomingBullet : IBulletData
{
    /// <summary> 追尾対象 </summary>
    private Transform _homingTarget = default;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public int GunnerLayer { get; set; }
    public Vector2 MoveForward { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void CashReset()
    {
        _homingTarget = null;
    }

    public void Movement()
    {
        //enemyを追尾するように直す
        if (_homingTarget == null)
        {
            //一番近くにいる敵を割り当てる
            var enemies = GameManager.Instance.GetEnemyManager().Enemies;
            if (enemies == null || enemies.Count <= 0)
            {
                MoveForward = Vector2.right;
                return;
            }

            var player = GameManager.Instance.PlayerTransform.position;
            _homingTarget = enemies[0].gameObject.transform;

            var currentClosedDist = Vector3.Distance(player, enemies[0].gameObject.transform.position);
            for (int i = 1; i < enemies.Count; i++)
            {
                var enemy = enemies[i].gameObject.transform;
                var distance = Vector3.Distance(player, enemy.position);
                if (distance < currentClosedDist)
                {
                    currentClosedDist = distance;
                    _homingTarget = enemy.transform;
                }
            }
        }

        Vector2 direction = _homingTarget.position - Transform.position;
        direction.Normalize();
        MoveForward = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);

        Rb2d.velocity = MoveForward * Speed;
    }

    public void Hit(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

        damageTarget.ReceiveDamage(AttackValue);
        GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
    }
}
