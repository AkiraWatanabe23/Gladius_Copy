using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShotSystem : EnemySystemBase
{
    private readonly List<Shot> _shotEnemies = default;

    public ShotSystem(EnemyCommon enemyCommon, params Shot[] targets)
    {
        EnemyCommon = enemyCommon;

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

    public override void AddEnemy(IEnemy target)
    {
        if (target is not Shot) { return; }

        _shotEnemies.Add((Shot)target);
    }

    public override void RemoveEnemy(IEnemy target)
    {
        if (target is not Shot) { return; }

        _shotEnemies.Remove((Shot)target);
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
        var go = EnemyCommon.ObjectPool.SpawnObject(EnemyCommon.BulletHolder.DefaultBullet);
        go.transform.position = target.Transform.position;
        var bullet = go.GetComponent<BulletController>();

        bullet.Intialize(1f, target.AttackValue, target.Enemy.layer);
        Debug.Log("attack!!");
    }
}
