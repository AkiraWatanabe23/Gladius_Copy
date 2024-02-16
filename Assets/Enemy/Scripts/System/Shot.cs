using UnityEngine;

public class Shot : EnemySystemBase
{
    private readonly int _attackValue = 0;
    private readonly float _attackInterval = 0f;
    private readonly float _searchAreaRadius = 0f;
    private readonly GameObject _enemy = default;

    private readonly Transform _enemyTransform = default;
    private readonly Transform _playerTransform = default;

    private Renderer _renderer = default;
    private float _attackTimer = 0f;

    private bool _isEnterArea = false;

    private bool IsEnterArea
    {
        get
        {
            if (_renderer == null) { _renderer = _enemy.GetComponent<Renderer>(); }

            var sqrDistance = (_playerTransform.position - _enemyTransform.position).sqrMagnitude;
            var isEnter = _renderer.isVisible && sqrDistance <= _searchAreaRadius * _searchAreaRadius;

            if (isEnter) { _isEnterArea = true; }

            return _isEnterArea;
        }
    }

    public Shot(GameObject go, Transform target, int attackValue, float searchRadius, float attackInterval)
    {
        _enemy = go;
        _playerTransform = target;
        _attackValue = attackValue;
        _searchAreaRadius = searchRadius;
        _attackInterval = attackInterval;

        _enemyTransform =_enemy.transform;
    }

    public override void EnemyMovement()
    {
        if (!IsEnterArea) { return; }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackInterval)
        {
            //攻撃（弾のPrefab生成）
            //var bullet = GameManager.Instance.ObjectPool.SpawnObject(_bulletPrefab);
            //if (bullet.TryGetComponent(out IBulletData bulletData))
            //{
            //    bulletData.Damage = _attackValue;
            //}

            Debug.Log("攻撃！！！");
            _attackTimer = 0f;
        }
    }
}
