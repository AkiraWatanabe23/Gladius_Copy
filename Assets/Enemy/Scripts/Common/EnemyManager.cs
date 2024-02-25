using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyCommon _enemyCommon = new();

    private EnemyMasterSystem _enemyMasterSystem = default;
    private EnemySystemUpdate _updateSystem = default;

    private void Awake()
    {
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
        yield return null;

        foreach (var enemy in enemies) { enemy.Initialize(); }

        _enemyMasterSystem = new(_enemyCommon, new ObjectPool());
        _enemyMasterSystem.Initialize(enemies);

        yield return null;
    }

    private void Loaded()
    {
        Debug.Log("Finish Initialized Enemy Systems");
        _updateSystem.SetupEnemyMasterSystem(_enemyMasterSystem);
        _updateSystem.enabled = true;
    }

    private void OnDestroy() => _enemyMasterSystem.OnDestroy();
}
