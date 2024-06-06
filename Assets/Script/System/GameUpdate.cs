using Constants;
using System;
using System.Linq;
using UnityEngine;

public class GameUpdate : MonoBehaviour
{
    private float _timer = 0f;
    private bool _isTimeMeasuring = false;
    private bool _isPause = false;
    private CameraController _camera = default;
    private EnemyManager _enemyManager = default;
    private Func<bool> _clearCondition = default;
    private Func<bool> _gameOverCondition = default;
    private GameObject[] _pausableObjects = default;

    protected bool GetPauseInput => Input.GetKeyDown(KeyCode.Escape);

    public float Timer => _timer;

    public void Initialize(
        CameraController camera, EnemyManager enemyManager,
        Func<bool> clearCondition, Func<bool> gameOverCondition, bool isTimeMeasuring = false)
    {
        _camera = camera;
        _enemyManager = enemyManager;
        _clearCondition = clearCondition;
        _gameOverCondition = gameOverCondition;
        _isTimeMeasuring = isTimeMeasuring;

        _pausableObjects = FindObjectsOfType<GameObject>().Where(go => go.TryGetComponent(out IPause _)).ToArray();
    }

    private void Update()
    {
        if (_isPause) { return; }

        _camera.OnUpdate(Time.deltaTime);
        _enemyManager.OnUpdate(Time.deltaTime);

        if (GetPauseInput)
        {
            if (_isPause) { Resume(); }
            else if (!_isPause) { Pause(); }
        }
        if (_isTimeMeasuring) { _timer += Time.deltaTime; }

        if (_gameOverCondition != null && _gameOverCondition()) { GameOver(); }
        if (_clearCondition != null && _clearCondition()) { GameClear(); }
    }

    public void Pause()
    {
        Consts.Log("Pause");
        _isPause = true;
        foreach (var pausable in _pausableObjects)
        {
            pausable.GetComponent<IPause>().Pause();
        }
    }

    public void Resume()
    {
        Consts.Log("Resume");
        _isPause = false;
        foreach (var pausable in _pausableObjects)
        {
            pausable.GetComponent<IPause>().Resume();
        }
    }

    private void GameClear()
    {
        Consts.Log("game clear");
        SceneLoader.FadeLoad(SceneName.Result);
    }

    private void GameOver()
    {
        Consts.Log("game over");
        SceneLoader.FadeLoad(SceneName.Result);
    }
}
