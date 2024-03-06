using UnityEngine;

/// <summary> 通常弾 </summary>
public class DefautBullet : IBulletData
{
    public GameObject BulletObj { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        Rb2d.velocity = Vector2.right * Speed;
    }

    public void Hit(GameObject hitTarget)
    {
        Debug.Log("hit");
        if (hitTarget == null) { return; }
        if (!hitTarget.TryGetComponent(out IDamageable damageTarget)) { return; }

        damageTarget.ReceiveDamage(AttackValue);
        Common.Instance.ObjectPool.RemoveObject(BulletObj);
    }
}
