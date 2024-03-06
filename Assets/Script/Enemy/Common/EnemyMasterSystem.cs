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
            new AssaultSystem(EnemyCommon, assaultEnemies.ToArray()),
            new ShotSystem(EnemyCommon, shotEnemies.ToArray()),
            new BossSystem(EnemyCommon, bossEnemies.ToArray())
        };
        foreach (var system in _enemySystems) { system.Initialize(); }
    }

    public void OnUpdate(float deltaTime)
    {
        if (_enemySystems.Length == 0) { return; }
        foreach (var enemy in _enemySystems) { enemy.OnUpdate(); }

        if (EnemyCommon.EnemySpawners != null && EnemyCommon.EnemySpawners.Length > 0)
        {
            foreach (var spawner in EnemyCommon.EnemySpawners) { spawner.Measuring(deltaTime); }
        }
    }

    public void OnDestroy()
    {
        if (_enemySystems.Length == 0) { return; }
        foreach (var enemy in _enemySystems) { enemy.OnDestroy(); }
    }

    public void AddEnemy(IEnemy enemy)
    {
        for (int i = 0; i <  _enemySystems.Length; i++) { _enemySystems[i].AddEnemy(enemy); }
    }

    public void RemoveEnemy(IEnemy enemy)
    {
        for (int i = 0; i < _enemySystems.Length; i++) { _enemySystems[i].RemoveEnemy(enemy); }
    }
}
