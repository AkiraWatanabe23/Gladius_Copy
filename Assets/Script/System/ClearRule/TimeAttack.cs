using UnityEngine;

/// <summary> タイムアタック（他条件と併用） </summary>
public class TimeAttack : IClearRule
{
    [Tooltip("制限時間")]
    [SerializeField]
    private float _clearTime = 100f;

    public void Init() { }

    public bool ClearCondition() => GameManager.Instance.Timer < _clearTime;
}
