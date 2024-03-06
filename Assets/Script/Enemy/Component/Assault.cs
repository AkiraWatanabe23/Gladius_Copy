using UnityEngine;

public class Assault : IEnemy
{
    [SerializeField]
    private int _hp = 1;
    [SerializeField]
    private int _attackValue = 1;
    [SerializeField]
    private float _moveSpeed = 1f;

    public int HP { get => _hp; set => _hp = value; }
    public int AttackValue => _attackValue;
    public float MoveSpeed => _moveSpeed;

    public Rigidbody2D Rb2d { get; set; }
    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }

    public void Init()
    {
        if (Enemy.TryGetComponent(out Rigidbody2D rb2d)) { Rb2d = rb2d; }
        else { Rb2d = Enemy.AddComponent<Rigidbody2D>(); }

        Rb2d.gravityScale = 0f;
    }
}
