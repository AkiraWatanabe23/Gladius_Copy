public abstract class EnemySystemBase
{
    public EnemyCommon EnemyCommon { get; set; }

    public virtual void Initialize() { }
    public virtual void OnUpdate() { }
    public virtual void OnDestroy() { }
}
