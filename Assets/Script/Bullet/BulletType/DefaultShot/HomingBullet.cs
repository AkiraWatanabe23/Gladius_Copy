using UnityEngine;

/// <summary> 追尾 </summary>
public class HomingBullet : IBulletData
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
        //enemyを追尾するように直す
        Vector2 direction = GameManager.Instance.PlayerTransform.position - Transform.position;
        direction.Normalize();
        MoveForward = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);

        Rb2d.velocity = MoveForward * Speed;
    }

    public void Hit(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

        damageTarget.ReceiveDamage(AttackValue);
        GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
    }
}
