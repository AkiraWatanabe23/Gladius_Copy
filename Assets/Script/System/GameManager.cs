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
    private EnemyMovementParams _enemyMovementParams = new();
    [SerializeField]
    private ItemSpawner _itemSpawner = default;
    [Tooltip("使用するプラスショット")]
    [SerializeField]
    private List<PlusShotType> _plusShots = default;
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
    public EnemyMovementParams EnemyMovementParams => _enemyMovementParams;
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
    public int ShotGunCount => _shotGunCount;
    public float SupportMoveSqrtDistance => _supportMoveDistance * _supportMoveDistance;
    public int MaxSupportCount => _maxSupportCount;
    public int CurrentSupportCount { get => _currentSupportCount; set => _currentSupportCount = value; }
    /// <summary> 補助兵装のList </summary>
    public List<Support> Supports { get; set; }
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
