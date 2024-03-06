using UnityEngine;

/// <summary> 貫通弾 </summary>
public class PenetrationBullet : IBulletData
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

    }
}
