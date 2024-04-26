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
        for (int i = _shotEnemies.Count - 1; i >= 0; i--)
        {
            if (_shotEnemies[i] == null) { continue; }
            Movement(_shotEnemies[i]);
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
            _jumping ??= new(EnemyManager);
            _jumping.Movement(target.Controller);
        }
    }
}
