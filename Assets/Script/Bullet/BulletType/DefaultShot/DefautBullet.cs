using UnityEngine;

/// <summary> 通常弾 </summary>
public class DefautBullet : IBulletData
{
    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public int GunnerLayer { get; set; }
    public Vector2 MoveForward { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        Rb2d.velocity = MoveForward * Speed;
    }

    public void Hit(Collider2D collision)
    {
        if (collision.gameObject.layer == GunnerLayer) { return; }

        IDamageable target = null;
        if (GunnerLayer == 6) //EnemyBulletがPlayerに
        {
            target = collision.gameObject.GetComponentInParent<IDamageable>();
            if (target == null) { return; }
        }
        else if (GunnerLayer == 3) //PlayerBulletがEnemyに
        {
            if (!collision.gameObject.TryGetComponent(out target)) { return; }
        }

        target.ReceiveDamage(AttackValue);
        Debug.Log("receive");
        GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
    }
}
