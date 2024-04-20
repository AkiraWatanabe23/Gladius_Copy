using Constants;
using System;
using UnityEngine;

public class GameUpdate : MonoBehaviour
{
    private float _timer = 0f;
    private bool _isTimeMeasuring = false;
    private EnemyManager _enemyManager = default;
    private Func<bool> _clearCondition = default;

    public float Timer => _timer;

    public void Initialize(EnemyManager enemyManager, Func<bool> clearCondition, bool isTimeMeasuring = false)
    {
        _enemyManager = enemyManager;
        _clearCondition = clearCondition;
        _isTimeMeasuring = isTimeMeasuring;
    }

    private void Update()
    {
        _enemyManager.OnUpdate(Time.deltaTime);
        if (_isTimeMeasuring) { _timer += Time.deltaTime; }
        if (_clearCondition != null && _clearCondition())
        {
            Consts.Log("game clear");
        }
    }
}
