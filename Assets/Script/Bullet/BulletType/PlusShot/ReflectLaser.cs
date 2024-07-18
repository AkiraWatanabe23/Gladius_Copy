using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary> 反射レーザー </summary>
public class ReflectLaser : IBulletData
{
    [Range(45f, 180f)]
    [SerializeField]
    private float _reflectAngle = 45f;

    public GameObject BulletObj { get; set; }
    public UnityEngine.Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public int GunnerLayer { get; set; }
    public Vector2 MoveForward { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public float ReflectAngle => _reflectAngle;

    public void Movement()
    {
        Rb2d.velocity = MoveForward * Speed;
    }

    public void Hit(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground _))
        {
            // 現在の速度ベクトルを取得
            Vector2 velocity = MoveForward;

            // 衝突点の取得
            Vector2 collisionPoint = collision.ClosestPoint(new Vector2(Transform.position.x, Transform.position.y));

            // 衝突した表面の法線ベクトルを取得
            Vector2 normal = (new Vector2(Transform.position.x, Transform.position.y) - collisionPoint).normalized;

            // 反射ベクトルを計算
            Vector2 reflect = Vector2.Reflect(velocity, normal);

            // 速度を維持して向きを変更
            MoveForward = reflect.normalized * velocity.magnitude;

            // GameObjectの向きを変更して反転する
            float angle = Mathf.Atan2(reflect.y, reflect.x) * Mathf.Rad2Deg;
            Transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

            damageTarget.ReceiveDamage(AttackValue);
            GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
        }
    }
}
