using System.Collections.Generic;

public class EnemyMasterSystem
{
    private EnemySystemBase[] _enemySystems = default;

    public EnemyCommon EnemyCommon { get; private set; }

    public EnemyMasterSystem(EnemyCommon enemyCommon, ObjectPool pool)
    {
        EnemyCommon = enemyCommon;
        enemyCommon.ObjectPool = pool;
    }

    public void Initialize(params EnemyController[] enemies)
    {
        List<Assault> assaultEnemies = new();
        List<Shot> shotEnemies = new();
        List<Boss> bossEnemies = new();
        foreach (var enemy in enemies)
        {
            switch (enemy.EnemySystem)
            {
                case Assault: assaultEnemies.Add((Assault)enemy.EnemySystem); break;
                case Shot: shotEnemies.Add((Shot)enemy.EnemySystem); break;
                case Boss: bossEnemies.Add((Boss)enemy.EnemySystem); break;
            }
        }

        _enemySystems = new EnemySystemBase[3]
        {
            new AssaultSystem(assaultEnemies.ToArray()),
            new ShotSystem(shotEnemies.ToArray()),
            new BossSystem(bossEnemies.ToArray())
        };
        foreach (var system in _enemySystems)
        {
            system.Initialize();
            system.EnemyCommon = EnemyCommon;
        }
    }

    public void OnUpdate()
    {
        if (_enemySystems.Length == 0) { return; }
        foreach (var enemy in _enemySystems) { enemy.OnUpdate(); }
    }

    public void OnDestroy()
    {
        if (_enemySystems.Length == 0) { return; }
        foreach (var enemy in _enemySystems) { enemy.OnDestroy(); }
    }
}
