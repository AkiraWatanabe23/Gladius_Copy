using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SubclassSelector]
    [SerializeReference]
    private IEnemy _enemySystem = default;

    private EnemyType _enemyType = EnemyType.None;

    public IEnemy EnemySystem => _enemySystem;

    public void Initialize()
    {
        _enemyType = _enemySystem switch
        {
            AssaultSystem => EnemyType.Assault,
            ShotSystem => EnemyType.Shot,
            BossSystem => EnemyType.Boss,
            _ => EnemyType.None,
        };
        _enemySystem.Enemy = gameObject;
        _enemySystem.Transform = gameObject.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _) &&
            collision.gameObject.TryGetComponent(out IDamageable player))
        {
            player.ReceiveDamage(_enemySystem.AttackValue);
            if (_enemyType == EnemyType.Assault) { Destroy(gameObject); }
        }
    }

    /// <summary> 画面外からいなくなったら呼び出される </summary>
    private void OnBecameInvisible() => Destroy(gameObject);

    public void ReceiveDamage(int value)
    {
        _enemySystem.HP -= value;
        if (_enemySystem.HP <= 0)
        {
            Debug.Log("やられたー");
            //Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_enemyType != EnemyType.Shot) { return; }

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
