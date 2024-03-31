using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _hp = 1;
    [SerializeField]
    private int _attackValue = 1;
    [SerializeField]
    private float _moveSpeed = 1f;

    [SubclassSelector]
    [SerializeReference]
    private IEnemy _enemySystem = default;
    [SerializeField]
    private EnemyMovementType _movementType = EnemyMovementType.None;

    private EnemyType _enemyType = EnemyType.None;

    public int HP { get => _hp; set => _hp = value; }
    public int AttackValue => _attackValue;
    public float MoveSpeed => _moveSpeed;
    public IEnemy EnemySystem => _enemySystem;
    public EnemyMovementType MovementType { get => _movementType; set => _movementType = value; }

    public void Initialize()
    {
        _enemyType = _enemySystem switch
        {
            Assault => EnemyType.Assault,
            Shot => EnemyType.Shot,
            Boss => EnemyType.Boss,
            _ => EnemyType.None,
        };
        _enemySystem.EnemyController = this;
        _enemySystem.Enemy = gameObject;
        _enemySystem.Transform = gameObject.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController _) &&
            collision.gameObject.TryGetComponent(out IDamageable player))
        {
            player.ReceiveDamage(_attackValue);
            if (_enemyType == EnemyType.Assault)
            {
                GameManager.Instance.ObjectPool.RemoveObject(gameObject);
            }
        }
    }

    /// <summary> 画面外からいなくなったら呼び出される </summary>
    private void OnBecameInvisible()
    {
        GameManager.Instance.ObjectPool.RemoveObject(gameObject);
    }

    public void ReceiveDamage(int value)
    {
        _hp -= value;
        if (_hp <= 0)
        {
            GameManager.Instance.EnemyDeadCount++;
            GameManager.Instance.ObjectPool.RemoveObject(gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_enemySystem is not Shot) { return; }

        var shot = (Shot)_enemySystem;

        var old = Gizmos.color;
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(gameObject.transform.position, shot.SearchRadius);
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
