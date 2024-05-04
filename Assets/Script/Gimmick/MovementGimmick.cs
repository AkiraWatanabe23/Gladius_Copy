using Constants;
using System.Collections.Generic;
using UnityEngine;

public class MovementGimmick : MonoBehaviour
{
    [Tooltip("移動経路（上から設定した順に動く）")]
    [SerializeField]
    private List<Transform> _routeList = default;
    [Tooltip("最終地点のみ別途設定")]
    [SerializeField]
    private Transform _goal = default;
    [Range(1f, 15f)]
    [SerializeField]
    private float _moveSpeed = 1f;
    [Tooltip("自機衝突時に与えるダメージ")]
    [SerializeField]
    private int _attackValue = 1;

    [Header("For Gizmos")]
    [SerializeField]
    private float _searchRadius = 1f;

    private Rigidbody2D _rb2d = default;
    private Vector2 _moveDirection = Vector2.zero;
    [SerializeField]
    private Transform[] _route = default;
    private int _nextTargetIndex = -1;
    private bool _isMoving = false;

    protected bool IsMovable
        => (GameManager.Instance.PlayerTransform.position - transform.position).sqrMagnitude <= _searchRadius * _searchRadius;

    private void Start()
    {
        if (!gameObject.TryGetComponent(out _rb2d)) { _rb2d = gameObject.AddComponent<Rigidbody2D>(); }
        _rb2d.isKinematic = true;

        RouteInitialize();
    }

    private void Update()
    {
        if (!_isMoving && IsMovable) { _isMoving = true; }
        if (_isMoving) { Movement(); }
    }

    private void RouteInitialize()
    {
        if (_goal == null) { Consts.LogWarning("Goalが未設定です"); return; }

        _route = new Transform[_routeList.Count + 2];
        _route[0] = transform;
        _route[^1] = _goal;
        for (int i = 1; i < _route.Length - 1; i++)
        {
            _route[i] = _routeList[i - 1];
        }
        _nextTargetIndex = 1;
    }

    private void Movement()
    {
        AudioManager.Instance.PlaySE(SEType.MovementGimmick);
        if (_nextTargetIndex >= _route.Length)
        {
            _isMoving = false;
            _rb2d.velocity = Vector2.zero;
            return;
        }
        _moveDirection = (_route[_nextTargetIndex].position - _route[_nextTargetIndex - 1].position).normalized;
        _rb2d.velocity = _moveDirection * _moveSpeed;

        if ((_route[_nextTargetIndex].position - transform.position).sqrMagnitude <= 1f)
        {
            _nextTargetIndex++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground _) || collision.gameObject.TryGetComponent(out MovementGimmick _))
        {
            GameManager.Instance.ObjectPool.RemoveObject(gameObject);
        }
        else if (collision.gameObject.TryGetComponent(out EnemyController _))
        {
            GameManager.Instance.ObjectPool.RemoveObject(collision.gameObject);
        }
        else if (collision.gameObject.TryGetComponent(out IDamageable target))
        {
            target.ReceiveDamage(_attackValue);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _searchRadius);
    }
#endif
}
