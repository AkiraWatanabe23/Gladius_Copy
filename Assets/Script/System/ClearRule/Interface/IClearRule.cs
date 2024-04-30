public interface IClearRule
{
    public ClearConditionName Condition { get; }

    public void Init();
    public bool ClearCondition();
}

public enum ClearConditionName
{
    None,
    ArriveGoal,
    DefeatBoss,
    EnemyAnnihilated,
    Survival,
    TimeAttack
}
