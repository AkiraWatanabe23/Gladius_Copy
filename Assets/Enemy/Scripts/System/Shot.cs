using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Shot : EnemySystemBase
{
    private readonly float _searchAreaRadius = 0f;
    private readonly GameObject _enemy = default;

    private readonly Transform _enemyTransform = default;
    private readonly Transform _playerTransform = default;

    private Renderer _renderer = default;

    private bool _isEnterArea = false;
    private bool _isMeasuring = false;

    public override int AttackValue { get; protected set; }
    public override float AttackInterval { get; protected set; }

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

    public Shot(GameObject go, Transform target, int attackValue, float attackInterval, float searchRadius)
    {
        _enemy = go;
        _playerTransform = target;

        AttackValue = attackValue;
        AttackInterval = attackInterval;

        _searchAreaRadius = searchRadius;
        _enemyTransform =_enemy.transform;
    }

    public override void OnUpdate()
    {
        if (!IsEnterArea) { return; }

        AttackMeasuring();
    }

    private async void AttackMeasuring()
    {
        if (_isMeasuring) { return; }

        _isMeasuring = true;
        await UniTask.Delay(TimeSpan.FromSeconds(AttackInterval));

        _isMeasuring = false;
        Attack();
    }

    private void Attack()
    {
        //ToDo
        //攻撃（弾のPrefab生成）
        //var bullet = GameManager.Instance.ObjectPool.SpawnObject(_bulletPrefab);
        //if (bullet.TryGetComponent(out IBulletData bulletData)) { bulletData.Damage = _attackValue; }
        Debug.Log("attack!!");
    }
}
