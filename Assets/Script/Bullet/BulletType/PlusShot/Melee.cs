using UnityEngine;

public class Melee : IBulletData
{
    [SerializeField]
    private float _serachAreaRadius = 1f;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement() { }

    public void Hit(Collider2D collision) { }
}
