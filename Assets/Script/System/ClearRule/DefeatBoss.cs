using UnityEngine;

/// <summary> Boss撃破 </summary>
public class DefeatBoss : IClearRule
{
    [SerializeField]
    private EnemyController _stageBoss = default;

    public void Init() { }

    public bool ClearCondition() => _stageBoss.HP <= 0;
}
