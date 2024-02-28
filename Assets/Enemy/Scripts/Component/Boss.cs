using UnityEngine;

public class Boss : IEnemy
{
    [SerializeField]
    private int _hp = 1;
    [SerializeField]
    private int _attackValue = 1;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _attackInterval = 1f;

    public int HP { get => _hp; set => _hp = value; }
    public int AttackValue => _attackValue;
    public float MoveSpeed => _moveSpeed;

    public float AttackInterval => _attackInterval;

    public bool IsMeasuring { get; set; }
    public Transform PlayerTransform { get; set; }

    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }
}
