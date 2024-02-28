using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab = default;
    [SerializeField]
    private Transform _spawnMuzzle = default;
    [SerializeField]
    private float _spawnInterval = 1f;

    private float _spawnTimer = 0f;

    protected Vector2 SpawnPos => _spawnMuzzle.position;
    /// <summary> 計測中 </summary>
    protected bool IsMeasuring => _spawnTimer <= _spawnInterval;

    public void Initialize()
    {
        if (_spawnMuzzle == null)
        {
            Debug.Log("生成位置の割り当てがなかったため、自オブジェクトを生成位置に設定します");
            _spawnMuzzle = transform;
        }
    }

    public void Measuring(float deltaTime)
    {
        _spawnTimer += deltaTime;
        if (IsMeasuring) { return; }

        EnemySpawn();
        _spawnTimer = 0f;
    }

    /// <summary> Prefab生成 </summary>
    private void EnemySpawn()
    {
        if (_enemyPrefab == null) { Debug.LogError("生成するオブジェクトの割り当てがありません"); return; }

        var enemy = EnemyManager.Instance.EnemyCommon.ObjectPool.SpawnObject(_enemyPrefab);
        enemy.transform.position = SpawnPos;

        var enemySystem = enemy.GetComponent<EnemyController>().EnemySystem;
        EnemyManager.Instance.EnemyMasterSystem.AddEnemy(enemySystem);
    }
}
