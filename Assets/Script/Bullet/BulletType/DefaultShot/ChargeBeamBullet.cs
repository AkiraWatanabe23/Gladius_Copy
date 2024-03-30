using UnityEngine;

/// <summary> チャージビーム </summary>
public class ChargeBeamBullet : IBulletData
{
    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        throw new System.NotImplementedException();
    }

    public void Hit(Collider2D collision)
    {
        throw new System.NotImplementedException();
    }
}
