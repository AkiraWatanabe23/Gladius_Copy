using System.Collections;
using UnityEngine;

public class Shot : IEnemy
{
    [SerializeField]
    private float _searchAreaRadius = 1f;
    [SerializeField]
    private float _attackInterval = 1f;
    [SerializeField]
    private Transform _shotMuzzle = default;

    private bool _isEnterArea = false;
    private Renderer _renderer = default;

    public float SearchRadius => _searchAreaRadius;
    public float AttackInterval => _attackInterval;
    public Transform ShotMuzzle => _shotMuzzle;

    public Transform PlayerTransform { get; set; }

    public EnemyController Controller { get; set; }
    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    #region Movement Unique Data
    public bool IsEnterArea
    {
        get
        {
            if (_renderer == null) { _renderer = Enemy.GetComponent<Renderer>(); }

            var sqrDistance = (PlayerTransform.position - Transform.position).sqrMagnitude;
            var isEnter = _renderer.isVisible && sqrDistance <= _searchAreaRadius * _searchAreaRadius;

            if (isEnter) { _isEnterArea = true; }
            else { _isEnterArea = false; }

            return _isEnterArea;
        }
    }
    public Vector2 MoveDirection
    {
        get
        {
            if (PlayerTransform.position.x - Transform.position.x > 0f) { return Vector2.right; }
            else { return Vector2.left; }
        }
    }
    public bool IsAiming { get; set; }
    public float AimingTimer { get; set; }
    public IEnumerator Measuring { get; set; }
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
