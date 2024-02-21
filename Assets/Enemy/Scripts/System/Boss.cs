using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Boss : EnemySystemBase
{
    private readonly GameObject _enemy = default;

    private readonly Transform _playerTransform = default;
    private readonly Transform _enemyTransform = default;

    private bool _isMeasuring = false;

    public override int AttackValue { get; protected set; }
    public override float AttackInterval { get; protected set; }

    public Boss(GameObject go, Transform playerTransform, int attackValue, float attackInterval)
    {
        _enemy = go;
        _playerTransform = playerTransform;
        AttackValue = attackValue;
        AttackInterval = attackInterval;

        _enemyTransform = _enemy.transform;

        //Bossの場合、Coreが設定されているか調べる
        EnemyCore enemyCore = null;
        for (int i = 0; i < _enemyTransform.childCount; i++)
        {
            if (_enemyTransform.GetChild(i).gameObject.TryGetComponent(out enemyCore)) { return; }
        }
        if (enemyCore == null) //EnemyCOreの設定がなかった場合は生成、設定する
        {
            var core = UnityEngine.Object.Instantiate(EnemyCommon.Instance.EnemyCorePrefab);
            core.transform.parent = _enemyTransform;
            core.transform.localPosition = Vector2.zero;
        }
    }

    public override void OnUpdate()
    {
        //縦方向のみPlayerに合わせて移動
        var velocity = _enemyTransform.position;
        velocity.y = _playerTransform.position.y;

        _enemyTransform.position = velocity;

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
        var bullet = EnemyCommon.Instance.ObjectPool.SpawnObject(EnemyCommon.Instance.BulletHolder.DefaultBullet);
        if (bullet.TryGetComponent(out IBulletData bulletData)) { bulletData.Speed = 1f; bulletData.Damage = AttackValue; }
        Debug.Log("attack!!");
    }
}
