using Constants;
using UnityEngine;

/// <summary> 敵機殲滅 </summary>
public class EnemyAnnihilated : IClearRule
{
    [Tooltip("ステージに出てくる敵の総数")]
    [SerializeField]
    private int _totalEnemyCount = 100;
    [ReadOnly]
    [SerializeField]
    private int _spawnedEnemyCount = 0;

    private EnemyManager _enemyManager = default;

    public ClearConditionName Condition => ClearConditionName.EnemyAnnihilated;
    public int TotalEnemyCount => _totalEnemyCount;
    public int SpawnedEnemyCount => _spawnedEnemyCount;

    public void Init() { }

    public void Init(EnemyManager enemyManager) => _enemyManager = enemyManager;

    public bool ClearCondition() => _enemyManager.Enemies.Count <= 0;

    /// <summary> 敵機の生成数を数える </summary>
    /// <param name="spawnCount"> 総生成数 </param>
    public bool SpawnedEnemy(int spawnCount)
    {
        //生成数をカウントし、上限に達した段階で終了する
        for (int i = 0; i < spawnCount; i++)
        {
            _spawnedEnemyCount++;
            if (_spawnedEnemyCount == _totalEnemyCount) { Consts.Log("これ以上敵を生成できません"); return false; }
        }
        return true;
    }
}
