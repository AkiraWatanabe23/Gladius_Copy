using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public class ShotSystem : EnemySystemBase
{
    private List<Shot> _shotEnemies = default;

    private CrawlGround _crawlGround = default;
    private Jumping _jumping = default;

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
            Movement(target);

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

    private void Movement(Shot target)
    {
        if (target.Controller.MovementType == EnemyMovementType.CrawlGround)
        {
            _crawlGround ??= new();
            _crawlGround.Movement(target.Controller);
        }
        if (target.Controller.MovementType == EnemyMovementType.Jumping)
        {
            _jumping ??= new();
            _jumping.Movement(target.Controller);
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
        var go = GameManager.Instance.ObjectPool.SpawnObject(
            GameManager.Instance.BulletHolder.BulletsDictionary[InitialBulletType.Default]);
        go.transform.position = target.Transform.position;
        var bullet = go.GetComponent<BulletController>();

        bullet.Initialize(2f, target.Controller.AttackValue, target.Enemy.layer);
    }
}
