public abstract class EnemySystemBase
{
    public EnemyManager EnemyManager { get; protected set; }

    public virtual void Initialize(EnemyManager enemyManager) { }
    public virtual void OnUpdate() { }
    public virtual void OnDestroy() { }
    public virtual void AddEnemy(IEnemy target) { }
    public virtual void RemoveEnemy(IEnemy target) { }
}
