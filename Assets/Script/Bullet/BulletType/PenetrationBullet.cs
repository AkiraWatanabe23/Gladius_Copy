using UnityEngine;

/// <summary> 貫通弾 </summary>
public class PenetrationBullet : IBulletData
{
    [Tooltip("貫通数")]
    [SerializeField]
    private int _penetrationCount = 1;

    public GameObject BulletObj { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    private int _hitCount = 0;

    public void Movement()
    {
        Rb2d.velocity = MoveDirection * Speed;
    }

    public void Hit(Collider2D collision)
    {
        if (collision.gameObject == null) { return; }
        if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

        damageTarget.ReceiveDamage(AttackValue);
        _hitCount++;

        if (_hitCount == _penetrationCount)
        {
            _hitCount = 0;
            EnemyManager.Instance.EnemyCommon.ObjectPool.RemoveObject(BulletObj);
        }
    }
}
