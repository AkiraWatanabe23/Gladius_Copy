using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossSystem : EnemySystemBase
{
    private readonly List<Boss> _bossEnemies = default;

    public BossSystem(params Boss[] targets)
    {
        _bossEnemies = new();
        _bossEnemies = targets.ToList();
        foreach (var target in targets) //初期化
        {
            target.PlayerTransform = EnemyCommon.Player.transform;

            //Bossの場合、Coreが設定されているか調べる
            EnemyCore enemyCore = null;
            for (int i = 0; i < target.Transform.childCount; i++)
            {
                if (target.Transform.GetChild(i).gameObject.TryGetComponent(out enemyCore)) { return; }
            }
            if (enemyCore == null) //EnemyCOreの設定がなかった場合は生成、設定する
            {
                var core = UnityEngine.Object.Instantiate(EnemyCommon.EnemyCorePrefab);
                core.transform.parent = target.Transform;
                core.transform.localPosition = Vector2.zero;
            }
        }
    }

    public override void OnUpdate()
    {
        foreach (var enemy in _bossEnemies)
        {
            //縦方向のみPlayerに合わせて移動
            var velocity = enemy.Transform.position;
            velocity.y = enemy.PlayerTransform.position.y;

            enemy.Transform.position = velocity;

            AttackMeasuring(enemy);
        }
    }

    private async void AttackMeasuring(Boss target)
    {
        if (target.IsMeasuring) { return; }

        target.IsMeasuring = true;
        await UniTask.Delay(TimeSpan.FromSeconds(target.AttackInterval));

        target.IsMeasuring = false;
        Attack(target);
    }

    private void Attack(Boss target)
    {
        var bullet = EnemyCommon.ObjectPool.SpawnObject(EnemyCommon.BulletHolder.DefaultBullet);
        if (bullet.TryGetComponent(out IBulletData bulletData)) { bulletData.Speed = 1f; bulletData.Damage = target.AttackValue; }
        Debug.Log("attack!!");
    }
}
