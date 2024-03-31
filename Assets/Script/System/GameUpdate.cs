using UnityEngine;

public class GameUpdate : MonoBehaviour
{
    private EnemyManager _enemyManager = default;

    public void Initialize(EnemyManager enemyManager) { _enemyManager = enemyManager; }

    private void Update()
    {
        _enemyManager.OnUpdate(Time.deltaTime);
    }
}
