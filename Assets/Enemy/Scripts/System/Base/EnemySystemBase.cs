public abstract class EnemySystemBase
{
    public virtual int AttackValue { get; protected set; }
    public virtual float AttackInterval { get; protected set; }

    public abstract void OnUpdate();
}
