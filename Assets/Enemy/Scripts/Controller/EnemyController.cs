using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour, IDamage
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

    [Header("For ShotEnemy")]
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
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.gravityScale = 0f;

        _enemySystem = _enemyType switch
        {
            EnemyType.Assault => new Assault(_moveSpeed, _rb2d),
            EnemyType.Shot => new Shot(gameObject, _player, _attackValue, _searchRadius, _attackInterval),
            EnemyType.Boss => new Boss(),
            _ => null
        };
    }

    private void Update()
    {
        if (_enemySystem == null) { Debug.Log("Systemの割り当てがありません"); return; }

        _enemySystem.EnemyMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _) &&
            collision.gameObject.TryGetComponent(out IDamage player))
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
}

public enum EnemyType
{
    None,
    Assault,
    Shot,
    Boss,
}
