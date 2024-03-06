using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SubclassSelector]
    [SerializeReference]
    private IEnemy _enemySystem = default;

    private EnemyType _enemyType = EnemyType.None;

    public IEnemy EnemySystem => _enemySystem;

    private void OnEnable() => Initialize();

    private void Initialize()
    {
        _enemyType = _enemySystem switch
        {
            Assault => EnemyType.Assault,
            Shot => EnemyType.Shot,
            Boss => EnemyType.Boss,
            _ => EnemyType.None,
        };
        _enemySystem.Enemy = gameObject;
        _enemySystem.Transform = gameObject.transform;

        _enemySystem.Init();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _) &&
            collision.gameObject.TryGetComponent(out IDamageable player))
        {
            player.ReceiveDamage(_enemySystem.AttackValue);
            if (_enemyType == EnemyType.Assault)
            {
                EnemyManager.Instance.EnemyCommon.ObjectPool.RemoveObject(gameObject);
            }
        }
    }

    /// <summary> 画面外からいなくなったら呼び出される </summary>
    private void OnBecameInvisible()
    {
        EnemyManager.Instance.EnemyCommon.ObjectPool.RemoveObject(gameObject);
    }

    private void OnDestroy() => EnemyManager.Instance.EnemyMasterSystem.RemoveEnemy(_enemySystem);

    public void ReceiveDamage(int value)
    {
        _enemySystem.HP -= value;
        if (_enemySystem.HP <= 0)
        {
            Debug.Log("やられたー");
            EnemyManager.Instance.EnemyCommon.ObjectPool.RemoveObject(gameObject);
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
