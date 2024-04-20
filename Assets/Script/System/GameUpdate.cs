using Constants;
using System;
using UnityEngine;

public class GameUpdate : MonoBehaviour
{
    private float _timer = 0f;
    private bool _isTimeMeasuring = false;
    private EnemyManager _enemyManager = default;
    private Func<bool> _clearCondition = default;
    private Func<bool> _gameOverCondition = default;

    public float Timer => _timer;

    public void Initialize(
        EnemyManager enemyManager, Func<bool> clearCondition, Func<bool> gameOverCondition, bool isTimeMeasuring = false)
    {
        _enemyManager = enemyManager;
        _clearCondition = clearCondition;
        _gameOverCondition = gameOverCondition;
        _isTimeMeasuring = isTimeMeasuring;
    }

    private void Update()
    {
        _enemyManager.OnUpdate(Time.deltaTime);
        if (_isTimeMeasuring) { _timer += Time.deltaTime; }
        if (_gameOverCondition != null && _gameOverCondition())
        {
            Consts.Log("game over");
        }
        if (_clearCondition != null && _clearCondition())
        {
            Consts.Log("game clear");
        }
    }
}
