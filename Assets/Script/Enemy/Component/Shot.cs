using UnityEngine;

public class Shot : IEnemy
{
    [SerializeField]
    private float _searchAreaRadius = 1f;
    [SerializeField]
    private float _attackInterval = 1f;

    private bool _isEnterArea = false;
    private Renderer _renderer = default;

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

    public EnemyController Controller { get; set; }
    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    #region Movement Unique Data
    public float Angle { get; set; }
    public Vector3 MoveInitPos { get; set; }
    #endregion

    public void Init()
    {
        if (Enemy.TryGetComponent(out Rigidbody2D rb2d)) { Rb2d = rb2d; }
        else { Rb2d = Enemy.AddComponent<Rigidbody2D>(); }

        Rb2d.gravityScale = 0f;
        MoveInitPos = Transform.position;
    }
}
