using UnityEngine;

public interface IBulletData
{
    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    /// <summary> 自身を撃ったオブジェクトのLayer </summary>
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    /// <summary> 初期化関数 </summary>
    public void Init(GameObject go, float speed, int attackValue, LayerMask gunner, Vector2 direction)
    {
        BulletObj = go;
        Transform = BulletObj.transform;
        Speed = speed;
        AttackValue = attackValue;
        GunnerLayer = gunner;
        MoveDirection = direction;

        if (go.TryGetComponent(out Rigidbody2D rb2d)) { Rb2d = rb2d; }
        else { Rb2d = go.AddComponent<Rigidbody2D>(); }

        Rb2d.isKinematic = true;
        Rb2d.gravityScale = 0f;

        if (MoveDirection == Vector2.zero)
        {
            Vector2 initialDirection = EnemyManager.Instance.EnemyCommon.Player.position - Transform.position;
            initialDirection.Normalize();
            MoveDirection = initialDirection;

            float angle = Mathf.Atan2(initialDirection.y, initialDirection.x) * Mathf.Rad2Deg;
            Transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
        }
    }

    public void Movement();

    /// <summary> オブジェクトとの衝突時に実行される関数 </summary>
    public void Hit(Collider2D collision);
}

/// <summary> 初期ショットに設定可能な弾の種類 </summary>
public enum InitialBulletType
{
    Default,
    Laser,
    ChargeBeam,
    Homing,
    ShotGun
}

/// <summary> アイテムで追加することができる弾の種類 </summary>
public enum PlusShotType
{
    Missile,
    TwoWay,
    SupportShot,
    ReflectLaser,
    Melee,
    Barrier
}
