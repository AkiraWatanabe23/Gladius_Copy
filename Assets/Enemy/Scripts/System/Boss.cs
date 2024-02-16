using UnityEngine;

public class Boss : EnemySystemBase
{
    private readonly float _attackInterval = 0f;
    private readonly Rigidbody2D _rb2d = default;

    private readonly Transform _playerTransform = default;

    private float _attackTimer = 0f;

    public Boss(Transform playerTransform, Rigidbody2D rb2d, float attackInterval)
    {
        _playerTransform = playerTransform;
        _rb2d = rb2d;
        _attackInterval = attackInterval;
    }

    public override void EnemyMovement()
    {
        //縦方向のみPlayerに合わせて移動
        var velocity = _rb2d.velocity;
        velocity.y = _playerTransform.position.y;

        _rb2d.velocity = velocity;


        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackInterval)
        {
            //攻撃（弾のPrefab生成）
            Debug.Log("攻撃！！！");
            _attackTimer = 0f;
        }
    }
}
