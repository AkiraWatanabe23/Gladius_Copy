using UnityEngine;

public class Assault : EnemySystemBase
{
    private readonly float _moveSpeed = 1f;
    private readonly Rigidbody2D _rb2d = default;

    public Assault(float moveSpeed, Rigidbody2D rb2d)
    {
        _moveSpeed = moveSpeed;
        _rb2d = rb2d;
    }

    public override void OnUpdate() => _rb2d.velocity = Vector2.left * _moveSpeed;
}
