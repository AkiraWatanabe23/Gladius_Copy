using UnityEngine;

/// <summary> ショットガン </summary>
public class ShotGun : IBulletData
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
        if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

        damageTarget.ReceiveDamage(AttackValue);
        GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
    }

    /// <summary> 自分の生存範囲外に出たときに呼ばれる </summary>
    public void AreaExit()
    {
        //todo 爆発エフェクト
        GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
    }
}
