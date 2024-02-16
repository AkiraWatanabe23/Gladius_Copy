using UnityEngine;

public class Boss : EnemySystemBase
{
    private readonly float _attackInterval = 0f;
    private readonly GameObject _enemy = default;

    private readonly Transform _playerTransform = default;
    private readonly Transform _enemyTransform = default;

    private float _attackTimer = 0f;

    public Boss(GameObject go, Transform playerTransform, float attackInterval)
    {
        _enemy = go;
        _playerTransform = playerTransform;
        _attackInterval = attackInterval;

        _enemyTransform = _enemy.transform;
    }

    public override void EnemyMovement()
    {
        //縦方向のみPlayerに合わせて移動
        var velocity = _enemyTransform.position;
        velocity.y = _playerTransform.position.y;

        _enemyTransform.position = velocity;

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackInterval)
        {
            //攻撃（弾のPrefab生成）
            Debug.Log("攻撃！！！");
            _attackTimer = 0f;
        }
    }
}
