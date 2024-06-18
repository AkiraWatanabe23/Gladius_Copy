using UnityEngine;

/// <summary> 反射レーザー </summary>
public class ReflectLaser : IBulletData
{
    [Range(45f, 180f)]
    [SerializeField]
    private float _reflectAngle = 45f;

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
        if (collision.gameObject.TryGetComponent(out Ground _))
        {
            var reflect = Vector2.Reflect(collision.gameObject.transform.position, Vector2.up);
            MoveForward = reflect;
        }
        else
        {
            if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

            damageTarget.ReceiveDamage(AttackValue);
            GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
        }
    }
}
