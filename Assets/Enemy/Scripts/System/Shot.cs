using UnityEngine;

public class Shot : EnemySystemBase
{
    private readonly int _attackValue = 0;
    private readonly float _attackInterval = 0f;
    private readonly float _searchAreaRadius = 0f;
    private readonly GameObject _enemy = default;
    private readonly Renderer _renderer = default;

    private readonly Transform _enemyTransform = default;
    private readonly Transform _playerTransform = default;

    private float _attackTimer = 0f;

    private bool IsEnterArea
    {
        get
        {
            if (_renderer == null) { _enemy.GetComponent<Renderer>(); }

            var sqrDistance = (_playerTransform.position - _enemyTransform.position).sqrMagnitude;

            return _renderer.isVisible && sqrDistance <= _searchAreaRadius * _searchAreaRadius;
        }
    }

    public Shot(GameObject go, GameObject target, int attackValue, float searchRadius, float attackInterval)
    {
        _enemy = go;
        _playerTransform = target.transform;
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
            _attackTimer = 0f;
        }
    }
}
