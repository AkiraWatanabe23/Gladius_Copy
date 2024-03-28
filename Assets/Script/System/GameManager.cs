using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player = default;
    [SerializeField]
    private ItemSpawner _itemSpawner = default;
    [ReadOnly]
    [SerializeField]
    private int _enemyDeadCount = 0;

    public PlayerController Player
    {
        get
        {
            if (_player == null) { _player = FindObjectOfType<PlayerController>(); }
            return _player;
        }
    }
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

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
}
