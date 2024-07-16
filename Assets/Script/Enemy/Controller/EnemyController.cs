using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _hp = 1;
    [SerializeField]
    private int _attackValue = 1;
    [Range(1f, 15f)]
    [SerializeField]
    private float _moveSpeed = 1f;
    [SubclassSelector]
    [SerializeReference]
    private IEnemy _enemySystem = default;
    [ReadOnly]
    [SerializeField]
    private EnemyMovementType _movementType = EnemyMovementType.None;

    [Header("For Debug")]
    [SerializeField]
    private PathDrawer _pathDrawer = new();

    public PathDrawer PathDrawer => _pathDrawer;

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
        _enemySystem.Controller = this;
        _enemySystem.Enemy = gameObject;
        _enemySystem.Transform = gameObject.transform;

        if (_enemyType == EnemyType.Boss) { GameManager.Instance.CameraController.AppearBoss(); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerCollider _))
        {
            var damageData = GameManager.Instance.Player.gameObject.GetComponent<IDamageable>();
            damageData.ReceiveDamage(_attackValue);
            if (_enemyType == EnemyType.Assault)
            {
                GameManager.Instance.GetEnemyManager().RemoveEnemy(_enemySystem);
                GameManager.Instance.ObjectPool.RemoveObject(gameObject);
            }
        }
    }

    /// <summary> 画面外からいなくなったら呼び出される </summary>
    private void OnBecameInvisible()
    {
        GameManager.Instance.GetEnemyManager().RemoveEnemy(_enemySystem);
        GameManager.Instance.ObjectPool.RemoveObject(gameObject);
    }

    public void ReceiveDamage(int value)
    {
        AudioManager.Instance.PlaySE(SEType.EnemyDamaged);
        _hp -= value;
        if (_hp <= 0)
        {
            Dead();
        }
        else
        {
            //ToDo : 点滅Animation
        }
    }

    private void Dead()
    {
        AudioManager.Instance.PlaySE(SEType.EnemyCrashed);
        GameManager.Instance.EnemyDeadPos = transform;
        GameManager.Instance.EnemyDeadCount++;
        GameManager.Instance.GetEnemyManager().RemoveEnemy(_enemySystem);
        GameManager.Instance.ObjectPool.RemoveObject(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_enemySystem is Assault assault)
        {
            if (_movementType != EnemyMovementType.FollowTerrain) { return; }
            if (assault.MoveRoute == null || assault.MoveRoute.Count <= 0) { return; }

            Gizmos.color = Color.green;
            for (int i = 0; i < assault.MoveRoute.Count - 1; i++)
            {
                Gizmos.DrawLine(assault.MoveRoute[i], assault.MoveRoute[i + 1]);
            }
        }
        else if (_enemySystem is Shot shot)
        {
            var old = Gizmos.color;
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(gameObject.transform.position, shot.SearchRadius);
            Gizmos.color = old;
        }
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
