/// <summary> 敵機殲滅 </summary>
public class EnemyAnnihilated : IClearRule
{
    private EnemyManager _enemyManager = default;

    public void Init() { }

    public void Init(EnemyManager enemyManager) => _enemyManager = enemyManager;

    public bool ClearCondition() => _enemyManager.Enemies.Count <= 0;
}
