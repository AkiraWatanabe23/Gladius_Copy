public class EnemyMasterSystem
{
    private readonly EnemySystemBase[] _enemySystems = default;

    public EnemyCommon EnemyCommon { get; private set; }

    public EnemyMasterSystem(EnemyCommon enemyCommon, ObjectPool pool, params EnemySystemBase[] systems)
    {
        EnemyCommon = enemyCommon;
        enemyCommon.ObjectPool = pool;

        _enemySystems = systems;
        for (int i = 0; i < _enemySystems.Length; i++)
        {
            _enemySystems[i].EnemyCommon = EnemyCommon;
        }
    }

    public void Initialize()
    {
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
        for (int i = 0; i < _enemySystems.Length; i++) { _enemySystems[i].AddEnemy(enemy); }
    }

    public void RemoveEnemy(IEnemy enemy)
    {
        for (int i = 0; i < _enemySystems.Length; i++) { _enemySystems[i].RemoveEnemy(enemy); }
    }
}
