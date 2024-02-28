using UnityEngine;

public class EnemySystemUpdate : MonoBehaviour
{
    private EnemyMasterSystem _enemyMasterSystem = default;

    public void SetupEnemyMasterSystem(EnemyMasterSystem masterSystem) => _enemyMasterSystem = masterSystem;

    private void Update() => _enemyMasterSystem.OnUpdate(Time.deltaTime);
}
