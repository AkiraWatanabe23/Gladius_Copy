using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyCommon _enemyCommon = new();

    private EnemyMasterSystem _enemyMasterSystem = default;
    private EnemySystemUpdate _updateSystem = default;

    public EnemyCommon EnemyCommon => _enemyCommon;
    public EnemyMasterSystem EnemyMasterSystem => _enemyMasterSystem;

    public ObjectPool ObjectPool { get; private set; }

    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }

        if (!TryGetComponent(out _updateSystem)) { _updateSystem = gameObject.AddComponent<EnemySystemUpdate>(); }
        _updateSystem.enabled = false;
    }

    private IEnumerator Start()
    {
        yield return Initialize();
        Loaded();
    }

    private IEnumerator Initialize()
    {
        var enemies = FindObjectsOfType<EnemyController>();
        var spawners = FindObjectsOfType<EnemySpawner>();
        yield return null;

        foreach (var enemy in enemies)
        {
            enemy.Initialize();
            SetUp(enemy.EnemySystem);
        }

        _enemyCommon.EnemySpawners = spawners;
        if (_enemyCommon.EnemySpawners != null && _enemyCommon.EnemySpawners.Length > 0)
        {
            foreach (var spawner in _enemyCommon.EnemySpawners) { spawner.Initialize(); }
        }

        _enemyMasterSystem = new(_enemyCommon, ObjectPool = new(), new AssaultSystem(), new ShotSystem(), new BossSystem());
        _enemyMasterSystem.Initialize();

        yield return null;
    }

    private void SetUp(IEnemy enemy)
    {
        switch (enemy)
        {
            case Assault:
                _enemyCommon.AssaultEnemies ??= new(); _enemyCommon.AssaultEnemies.Add((Assault)enemy); break;
            case Shot:
                _enemyCommon.ShotEnemies ??= new(); _enemyCommon.ShotEnemies.Add((Shot)enemy); break;
            case Boss:
                _enemyCommon.BossEnemies ??= new(); _enemyCommon.BossEnemies.Add((Boss)enemy); break;
        }
    }

    private void Loaded()
    {
        Debug.Log("Finish Initialized Enemy Systems");
        _updateSystem.SetupEnemyMasterSystem(_enemyMasterSystem);
        _updateSystem.enabled = true;
    }

    private void OnDestroy() => _enemyMasterSystem.OnDestroy();
}
