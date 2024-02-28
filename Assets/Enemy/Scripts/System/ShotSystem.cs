using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShotSystem : EnemySystemBase
{
    private readonly List<Shot> _shotEnemies = default;

    public ShotSystem(params Shot[] targets)
    {
        _shotEnemies = new();
        _shotEnemies = targets.ToList();
        foreach (var target in targets)
        {
            //ここで必要な値の初期化処理
            target.PlayerTransform = EnemyCommon.Player;
        }
    }

    public override void OnUpdate()
    {
        foreach (var target in _shotEnemies)
        {
            if (!target.IsEnterArea) { continue; }

            AttackMeasuring(target);
        }
    }

    private async void AttackMeasuring(Shot target)
    {
        if (target.IsMeasuring) { return; }

        target.IsMeasuring = true;
        await UniTask.Delay(TimeSpan.FromSeconds(target.AttackInterval));

        target.IsMeasuring = false;
        Attack(target);
    }

    private void Attack(Shot target)
    {
        var bullet = EnemyCommon.ObjectPool.SpawnObject(EnemyCommon.BulletHolder.DefaultBullet);
        bullet.transform.position = target.Transform.position;

        if (bullet.TryGetComponent(out IBulletData bulletData))
        {
            bulletData.Speed = 1f;
            bulletData.Damage = target.AttackValue;
        }
        Debug.Log("attack!!");
    }
}
