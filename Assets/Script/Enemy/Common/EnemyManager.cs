using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyManager
{
    [SerializeField]
    private GameObject _enemyCorePrefab = default;
    [SerializeField]
    private EnemySpawner[] _enemySpawners = default;
    [SerializeField]
    private List<EnemyController> _enemies = default;

    private AssaultSystem _assaultSystem = default;
    private ShotSystem _shotSystem = default;
    private BossSystem _bossSystem = default;
    private EnemySystemBase[] _enemySystems = default;

    public GameObject EnemyCorePrefab => _enemyCorePrefab;
    public EnemySpawner[] EnemySpawners { get => _enemySpawners; private set => _enemySpawners = value; }
    public List<EnemyController> Enemies => _enemies;
    public Transform PlayerTransform { get; private set; }

    public IEnumerator Initialize(EnemyController[] enemies, EnemySpawner[] spawners, Transform playerTransform)
    {
        PlayerTransform = playerTransform;

        _assaultSystem = new AssaultSystem();
        _shotSystem = new ShotSystem();
        _bossSystem = new BossSystem();
        _enemySystems = new EnemySystemBase[] { _assaultSystem, _shotSystem, _bossSystem };
        for (int i = 0; i < _enemySystems.Length; i++) { _enemySystems[i].Initialize(this); }

        _enemies = new();
        foreach (var enemy in enemies)
        {
            _enemies.Add(enemy);
            enemy.Initialize();
            AddEnemy(enemy.EnemySystem);
        }

        _enemySpawners = spawners;
        if (_enemySpawners != null && _enemySpawners.Length > 0)
        {
            foreach (var spawner in _enemySpawners) { spawner.Initialize(this); }
        }

        yield return null;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_enemySystems.Length == 0) { return; }
        foreach (var enemy in _enemySystems) { enemy.OnUpdate(); }

        if (_enemySpawners != null && _enemySpawners.Length > 0)
        {
            foreach (var spawner in _enemySpawners) { spawner.Measuring(deltaTime); }
        }
    }

    public void OnDestroy()
    {
        if (_enemySystems.Length == 0) { return; }
        foreach (var enemy in _enemySystems) { enemy.OnDestroy(); }
    }

    public void AddEnemy(IEnemy target)
    {
        switch (target)
        {
            case Assault: _assaultSystem.AddEnemy(target); break;
            case Shot: _shotSystem.AddEnemy(target); break;
            case Boss: _bossSystem.AddEnemy(target); break;
        }
    }

    public void RemoveEnemy(IEnemy target)
    {
        switch (target)
        {
            case Assault: _assaultSystem.RemoveEnemy(target); break;
            case Shot: _shotSystem.RemoveEnemy(target); break;
            case Boss: _bossSystem.RemoveEnemy(target); break;
        }
    }
}
