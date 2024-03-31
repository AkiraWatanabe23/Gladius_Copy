using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BossSystem : EnemySystemBase
{
    private List<Boss> _bossEnemies = default;

    public override void Initialize(EnemyManager enemyManager)
    {
        EnemyManager = enemyManager;

        if (_bossEnemies == null || _bossEnemies.Count <= 0) { return; }
        foreach (var target in _bossEnemies) //初期化
        {
            target.PlayerTransform = EnemyManager.PlayerTransform;

            //Bossの場合、Coreが設定されているか調べる
            EnemyCore enemyCore = null;
            for (int i = 0; i < target.Transform.childCount; i++)
            {
                if (target.Transform.GetChild(i).gameObject.TryGetComponent(out enemyCore)) { return; }
            }
            if (enemyCore == null) //EnemyCoreの設定がなかった場合は生成、設定する
            {
                var core = UnityEngine.Object.Instantiate(EnemyManager.EnemyCorePrefab);
                core.transform.parent = target.Transform;
                core.transform.localPosition = Vector2.zero;
            }
        }
    }

    public override void OnUpdate()
    {
        if (_bossEnemies == null || _bossEnemies.Count <= 0) { return; }
        foreach (var enemy in _bossEnemies)
        {
            //縦方向のみPlayerに合わせて移動
            var velocity = enemy.Transform.position;
            velocity.y = enemy.PlayerTransform.position.y;

            enemy.Transform.position = velocity;

            AttackMeasuring(enemy);
        }
    }

    public override void AddEnemy(IEnemy target)
    {
        _bossEnemies ??= new();
        _bossEnemies.Add((Boss)target);

        var enemy = target as Boss;
        enemy.PlayerTransform = EnemyManager.PlayerTransform;
        enemy.Init(EnemyManager.PlayerTransform, EnemyManager.EnemyCorePrefab);
    }

    public override void RemoveEnemy(IEnemy target) { _bossEnemies.Remove((Boss)target); }

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
        var bullet = GameManager.Instance.ObjectPool.SpawnObject(
            GameManager.Instance.BulletHolder.BulletsDictionary[InitialBulletType.Default]);
        if (bullet.TryGetComponent(out BulletController bulletData))
        {
            bulletData.Initialize(1f, target.EnemyController.AttackValue, target.Enemy.layer, Vector2.left);
        }
        Debug.Log("attack!!");
    }
}
