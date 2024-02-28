using UnityEngine;

public class Shot : IEnemy
{
    [SerializeField]
    private int _hp = 1;
    [SerializeField]
    private int _attackValue = 1;
    [SerializeField]
    private float _moveSpeed = 1f;
    [SerializeField]
    private float _searchAreaRadius = 1f;
    [SerializeField]
    private float _attackInterval = 1f;

    private bool _isEnterArea = false;
    private Renderer _renderer = default;

    public int HP { get => _hp; set => _hp = value; }
    public int AttackValue => _attackValue;
    public float MoveSpeed => _moveSpeed;
    public float SearchRadius => _searchAreaRadius;
    public float AttackInterval => _attackInterval;

    public bool IsEnterArea
    {
        get
        {
            if (_renderer == null) { _renderer = Enemy.GetComponent<Renderer>(); }

            var sqrDistance = (PlayerTransform.position - Transform.position).sqrMagnitude;
            var isEnter = _renderer.isVisible && sqrDistance <= _searchAreaRadius * _searchAreaRadius;

            if (isEnter) { _isEnterArea = true; }

            return _isEnterArea;
        }
    }

    public bool IsMeasuring { get; set; }
    public Transform PlayerTransform { get; set; }

    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }
}
