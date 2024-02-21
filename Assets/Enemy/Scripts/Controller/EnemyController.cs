using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyType _enemyType = EnemyType.None;
    [Min(1)]
    [SerializeField]
    private int _hp = 1;
    [Min(1)]
    [SerializeField]
    private int _attackValue = 1;
    [Min(1f)]
    [SerializeField]
    private float _moveSpeed = 1f;

    [Header("Shot Enemy")]
    [SerializeField]
    private GameObject _player = default;
    [SerializeField]
    private float _searchRadius = 1f;
    [SerializeField]
    private float _attackInterval = 1f;

    private Rigidbody2D _rb2d = default;
    private EnemySystemBase _enemySystem = default;

    private void Start()
    {
        if (_enemyType == EnemyType.Assault)
        {
            if (!TryGetComponent(out _rb2d)) { _rb2d = gameObject.AddComponent<Rigidbody2D>(); }
            _rb2d.gravityScale = 0f;
        }

        _enemySystem = _enemyType switch
        {
            EnemyType.Assault => new Assault(_moveSpeed, _rb2d),
            EnemyType.Shot => new Shot(gameObject, _player.transform, _attackValue, _attackInterval, _searchRadius),
            EnemyType.Boss => new Boss(gameObject, _player.transform, _attackValue, _attackInterval),
            _ => null
        };
    }

    private void Update()
    {
        if (_enemySystem == null) { Debug.Log("Systemの割り当てがありません"); return; }

        _enemySystem.OnUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _) &&
            collision.gameObject.TryGetComponent(out IDamageable player))
        {
            player.ReceiveDamage(_attackValue);
            if (_enemyType == EnemyType.Assault) { Destroy(gameObject); }
        }
    }

    /// <summary> 画面外からいなくなったら呼び出される </summary>
    private void OnBecameInvisible() => Destroy(gameObject);

    public void ReceiveDamage(int value)
    {
        _hp -= value;
        if (_hp <= 0)
        {
            Debug.Log("やられたー");
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_enemyType != EnemyType.Shot) { return; }

        var old = Gizmos.color;
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(gameObject.transform.position, _searchRadius);
        Gizmos.color = old;
    }
#endif
}

public enum EnemyType
{
    None,
    Assault,
    Shot,
    Boss,
}
