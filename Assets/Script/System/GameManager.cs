using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player = default;
    [SerializeField]
    private EnemyManager _enemyManager = new();
    [SerializeField]
    private ItemSpawner _itemSpawner = default;
    [Tooltip("使用するプラスショット")]
    [SerializeField]
    private List<PlusShotType> _plusShots = default;
    [Tooltip("インゲームに出てくる弾やショットのリスト")]
    [SerializeField]
    private BulletHolder _bulletHolder = new();
    [Tooltip("倒したEnemyの数")]
    [ReadOnly]
    [SerializeField]
    private int _enemyDeadCount = 0;

    private Transform _playerTransform = default;
    private GameUpdate _inGameUpdate = default;

    #region public Properties
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
    public BulletHolder BulletHolder => _bulletHolder;
    public int EnemyDeadCount
    {
        get => _enemyDeadCount;
        set
        {
            _enemyDeadCount = value;
            var remainder = _enemyDeadCount % 5;

            if (remainder == 0) { _itemSpawner.Spawn(remainder); }
        }
    }
    public ObjectPool ObjectPool { get; private set; }

    public static GameManager Instance { get; private set; }
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
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

        //もしシーン上にEnemy, Spawnerが存在したら実行を管理するclassに渡す
        yield return _enemyManager.Initialize(
            FindObjectsOfType<EnemyController>(), FindObjectsOfType<EnemySpawner>(), _player.transform);

        ObjectPool = new();

        yield return null;
    }

    private void Loaded()
    {
        Consts.Log("Finish Initialized");
        _inGameUpdate.Initialize(_enemyManager);
        _inGameUpdate.enabled = true;
    }
}
