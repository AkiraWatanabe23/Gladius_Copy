using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShotSystem : EnemySystemBase
{
    private List<Shot> _shotEnemies = default;

    public override void Initialize(EnemyManager enemyManager)
    {
        EnemyManager = enemyManager;

        if (_shotEnemies == null || _shotEnemies.Count <= 0) { return; }
        foreach (var target in _shotEnemies)
        {
            //ここで必要な値の初期化処理
            target.PlayerTransform = EnemyManager.PlayerTransform;
        }
    }

    public override void OnUpdate()
    {
        if (_shotEnemies == null || _shotEnemies.Count <= 0) { return; }
        foreach (var target in _shotEnemies)
        {
            if (!target.IsEnterArea) { continue; }

            AttackMeasuring(target);
        }
    }

    public override void AddEnemy(IEnemy target)
    {
        _shotEnemies ??= new();
        _shotEnemies.Add((Shot)target);

        var enemy = target as Shot;
        enemy.PlayerTransform = EnemyManager.PlayerTransform;
        enemy.Init();
    }

    public override void RemoveEnemy(IEnemy target) { _shotEnemies.Remove((Shot)target); }

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
        var go = GameManager.Instance.ObjectPool.SpawnObject(
            GameManager.Instance.BulletHolder.BulletsDictionary[InitialBulletType.Default]);
        go.transform.position = target.Transform.position;
        var bullet = go.GetComponent<BulletController>();
        var moveVector = Vector2.right;
        //var moveVector = new Vector2(1, -1);

        bullet.Initialize(2f, target.EnemyController.AttackValue, target.Enemy.layer, moveVector);
    }
}
