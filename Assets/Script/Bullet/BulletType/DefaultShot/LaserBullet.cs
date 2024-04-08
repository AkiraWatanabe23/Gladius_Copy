using UnityEngine;

/// <summary> レーザー（貫通） </summary>
public class LaserBullet : IBulletData
{
    [Tooltip("貫通数")]
    [SerializeField]
    private int _penetrationCount = 5;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        Rb2d.velocity = MoveDirection * Speed;
    }

    public void Hit(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

        damageTarget.ReceiveDamage(AttackValue);
        _penetrationCount--;

        if (_penetrationCount <= 0)
        {
            _penetrationCount = 5;
            GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
        }
    }
}
