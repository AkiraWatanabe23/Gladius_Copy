using Constants;
using System;
using System.Linq;
using UnityEngine;

public class GameUpdate : MonoBehaviour
{
    private float _timer = 0f;
    private bool _isTimeMeasuring = false;
    private bool _isPause = false;
    private EnemyManager _enemyManager = default;
    private Func<bool> _clearCondition = default;
    private Func<bool> _gameOverCondition = default;
    private GameObject[] _pausableObjects = default;

    protected bool GetPauseInput => Input.GetKeyDown(KeyCode.Escape);

    public float Timer => _timer;

    public void Initialize(
        EnemyManager enemyManager, Func<bool> clearCondition, Func<bool> gameOverCondition, bool isTimeMeasuring = false)
    {
        _enemyManager = enemyManager;
        _clearCondition = clearCondition;
        _gameOverCondition = gameOverCondition;
        _isTimeMeasuring = isTimeMeasuring;

        _pausableObjects = FindObjectsOfType<GameObject>().Where(go => go.TryGetComponent(out IPause _)).ToArray();
    }

    private void Update()
    {
        if (_isPause) { return; }

        _enemyManager.OnUpdate(Time.deltaTime);
        if (GetPauseInput)
        {
            _isPause = true;
            foreach (var pausable in  _pausableObjects)
            {
                pausable.GetComponent<IPause>().Pause();
            }
        }
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

    public void Resume()
    {
        _isPause = false;
        foreach (var pausable in _pausableObjects)
        {
            pausable.GetComponent<IPause>().Resume();
        }
    }
}
