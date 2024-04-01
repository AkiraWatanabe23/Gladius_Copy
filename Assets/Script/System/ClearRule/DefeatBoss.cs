public class DefeatBoss : IClearRule
{
    private EnemyController _stageBoss = default;

    public void Init(EnemyController boss)
    {
        _stageBoss = boss;
    }

    public bool ClearCondition()
    {
        return _stageBoss.HP <= 0;
    }
}
