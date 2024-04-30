using UnityEngine;

/// <summary> ゴール到達 </summary>
public class ArriveGoal : IClearRule
{
    [SerializeField]
    private Transform _goalPos = default;

    private Transform _playerTransform = default;
    public ClearConditionName Condition => ClearConditionName.ArriveGoal;

    public void Init()
    {
        _playerTransform = GameManager.Instance.PlayerTransform;
    }

    public bool ClearCondition()
    {
        if (_playerTransform == null || _goalPos == null) { return false; }

        return _playerTransform.position.x >= _goalPos.position.x;
    }
}
