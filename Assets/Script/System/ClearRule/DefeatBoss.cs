using UnityEngine;

/// <summary> Boss撃破 </summary>
public class DefeatBoss : IClearRule
{
    [SerializeField]
    private EnemyController _stageBoss = default;
    public ClearConditionName Condition => ClearConditionName.DefeatBoss;

    public void Init() { }

    public bool ClearCondition() => _stageBoss != null && _stageBoss.HP <= 0;

    public void SettingBoss(EnemyController boss) => _stageBoss = boss;
}
