﻿using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Serialized Members
    [Tooltip("同じ条件を重複させないように")]
    [SubclassSelector]
    [SerializeReference]
    private IClearRule[] _clearConditions = default;
    [Tooltip("ゲームの経過時間（タイムがクリア条件にかかわる場合に使う）")]
    [ReadOnly]
    [SerializeField]
    private float _timer = 0f;
    [SerializeField]
    private CameraController _cameraController = default;
    [SerializeField]
    private PlayerController _player = default;
    [SerializeField]
    private Transform _playerTransform = default;
    [SerializeField]
    private EnemyManager _enemyManager = default;
    [SerializeField]
    private ItemSpawner _itemSpawner = default;
    [SerializeField]
    private UIController _uiController = default;
    [Tooltip("インゲームに出てくる弾やショットのリスト")]
    [SerializeField]
    private BulletHolder _bulletHolder = new();
    [ReadOnly]
    [Tooltip("倒したEnemyの数")]
    [SerializeField]
    private int _enemyDeadCount = 0;
    [Tooltip("ショットガンを撃つときの一度に撃ちだす数")]
    [SerializeField]
    private int _shotGunCount = 3;
    [Range(1f, 10f)]
    [Tooltip("補助兵装が後続する距離")]
    [SerializeField]
    private float _supportMoveDistance = 1f;
    [SerializeField]
    private int _maxSupportCount = 5;
    [ReadOnly]
    [Tooltip("補助兵装の数")]
    [SerializeField]
    private int _currentSupportCount = 0;
    #endregion

    private readonly GameOver _gameOver = new();

    private bool _isTimeMeasuring = false;
    private GameUpdate _inGameUpdate = default;
    private EnemyAnnihilated _enemyAnnihilated = default;
    private DefeatBoss _defeatBoss = default;

    #region public Properties
    public IClearRule[] ClearConditions => _clearConditions;
    public float Timer => _timer = _inGameUpdate.Timer;
    public PlayerController Player
    {
        get
        {
            if (_player == null) { _player = FindObjectOfType<PlayerController>(); }
            return _player;
        }
    }
    public Transform PlayerTransform
    {
        get
        {
            if (_playerTransform == null) { _playerTransform = _player.transform; }
            return _playerTransform;
        }
    }
    public CameraController CameraController => _cameraController;
    public UIController UIController => _uiController;
    public GameUpdate InGameUpdate => _inGameUpdate;
    public EnemyAnnihilated EnemyAnnihilated => _enemyAnnihilated;
    public DefeatBoss DefeatBoss => _defeatBoss;
    public BulletHolder BulletHolder => _bulletHolder;
    public Transform EnemyDeadPos { get; set; }
    public int EnemyDeadCount
    {
        get => _enemyDeadCount;
        set
        {
            _enemyDeadCount = value;
            var remainder = _enemyDeadCount % _itemSpawner.ItemSpawnMultiple;

            if (remainder == 0) { _itemSpawner.Spawn(EnemyDeadPos); }
        }
    }
    public int ShotGunCount => _shotGunCount;
    public float SupportMoveSqrtDistance => _supportMoveDistance * _supportMoveDistance;
    public int MaxSupportCount => _maxSupportCount;
    public int CurrentSupportCount { get => _currentSupportCount; set => _currentSupportCount = value; }
    /// <summary> 補助兵装のList </summary>
    public List<Support> Supports { get; set; }
    /// <summary> メレー弾のList </summary>
    public List<Melee> Melees { get; set; }
    public ObjectPool ObjectPool { get; private set; }

    public static GameManager Instance { get; private set; }
    #endregion

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    private IEnumerator Start()
    {
        yield return Initialize();

        //Fade.Instance.StartFadeIn();
        Loaded();
    }

    private IEnumerator Initialize()
    {
        if (!TryGetComponent(out _inGameUpdate)) { _inGameUpdate = gameObject.AddComponent<GameUpdate>(); }
        _inGameUpdate.enabled = false;

        if (_cameraController == null) { _cameraController = FindObjectOfType<CameraController>(); }
        _cameraController?.Initialize(_player.gameObject);
        yield return null;

        if (_uiController == null) { _uiController = FindObjectOfType<UIController>(); }

        //もしシーン上にEnemy, Spawnerが存在したら実行を管理するclassに渡す
        yield return _enemyManager.Initialize(
            FindObjectsOfType<EnemyController>(), FindObjectsOfType<EnemySpawner>(), _player.transform);

        ObjectPool = new();
        yield return null;

        _gameOver.Init();
        if (_clearConditions == null || _clearConditions.Length <= 0) { yield break; }
        for (int i = 0; i < _clearConditions.Length; i++)
        {
            _clearConditions[i]?.Init();
            if (_clearConditions[i] is EnemyAnnihilated)
            {
                _enemyAnnihilated = _clearConditions[i] as EnemyAnnihilated;
                _enemyAnnihilated.Init(_enemyManager);
            }
            else if (_clearConditions[i] is Survival || _clearConditions[i] is TimeAttack)
            {
                _isTimeMeasuring = true;
            }
            else if (_clearConditions[i] is DefeatBoss)
            {
                _defeatBoss = _clearConditions[i] as DefeatBoss;
            }
        }
    }

    private void Loaded()
    {
        Consts.Log("Finish Initialized");
        _inGameUpdate.Initialize(
            _cameraController, _enemyManager, GameClear, () => _gameOver.GameOverCondition(), _isTimeMeasuring);
        _inGameUpdate.enabled = true;
    }

    private bool GameClear()
    {
        if (_clearConditions == null || _clearConditions.Length <= 0) { return false; }

        return _clearConditions.All(condition => condition.ClearCondition());
    }

    public EnemyManager GetEnemyManager() => _enemyManager;
}
