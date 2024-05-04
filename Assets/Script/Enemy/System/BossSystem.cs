using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BossSystem : EnemySystemBase
{
    private Boss _bossEnemy = default;

    public override void Initialize(EnemyManager enemyManager)
    {
        EnemyManager = enemyManager;
        if (_bossEnemy == null) { return; }

        _bossEnemy.PlayerTransform = EnemyManager.PlayerTransform;

        //Bossの場合、Coreが設定されているか調べる
        EnemyCore enemyCore = null;
        for (int i = 0; i < _bossEnemy.Transform.childCount; i++)
        {
            if (_bossEnemy.Transform.GetChild(i).gameObject.TryGetComponent(out enemyCore)) { return; }
        }
        if (enemyCore == null) //EnemyCoreの設定がなかった場合は生成、設定する
        {
            var core = UnityEngine.Object.Instantiate(EnemyManager.EnemyCorePrefab);
            core.transform.parent = _bossEnemy.Transform;
            core.transform.localPosition = Vector2.zero;
        }
    }

    public override void OnUpdate()
    {
        if (_bossEnemy == null) { return; }

        //縦方向のみPlayerに合わせて移動
        var velocity = _bossEnemy.Transform.position;
        velocity.y = _bossEnemy.PlayerTransform.position.y;

        _bossEnemy.Transform.position = velocity;

        AttackMeasuring(_bossEnemy);
    }

    public override void AddEnemy(IEnemy target)
    {
        _bossEnemy = (Boss)target;

        _bossEnemy.PlayerTransform = EnemyManager.PlayerTransform;
        _bossEnemy.Init(EnemyManager.PlayerTransform, EnemyManager.EnemyCorePrefab);
    }

    private async void AttackMeasuring(Boss target)
    {
        if (target.IsMeasuring) { return; }

        target.IsMeasuring = true;
        await UniTask.Delay(TimeSpan.FromSeconds(target.AttackInterval));

        target.IsMeasuring = false;
        Attack(target);
    }

    private void Attack(Boss target)
    {
        AudioManager.Instance.PlaySE(SEType.EnemyLaser);
        var bullet = GameManager.Instance.ObjectPool.SpawnObject(
            GameManager.Instance.BulletHolder.BulletsDictionary[InitialBulletType.Laser]);
        if (bullet.TryGetComponent(out BulletController bulletData))
        {
            bulletData.Initialize(target.Controller.AttackValue, target.Enemy.layer);
        }
        Debug.Log("attack!!");
    }
}
