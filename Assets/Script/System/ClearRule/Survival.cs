using UnityEngine;

/// <summary> 生き残り </summary>
public class Survival : IClearRule
{
    [Tooltip("生き残る時間（クリア条件：サバイバルの場合）")]
    [SerializeField]
    private float _survivalTime = 100f;

    public ClearConditionName Condition => ClearConditionName.Survival;

    public void Init() { }

    public bool ClearCondition() => GameManager.Instance.Timer >= _survivalTime;
}
