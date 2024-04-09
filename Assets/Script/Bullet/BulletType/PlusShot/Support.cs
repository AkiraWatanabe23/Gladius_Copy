using UnityEngine;

public class Support : IBulletData
{
    private Transform _player = default;
    /// <summary> 自分が何番目の補助兵装か </summary>
    private int _supportIndex = -1;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        _player ??= GameManager.Instance.PlayerTransform;
        if (_supportIndex <= 0) { _supportIndex = GameManager.Instance.CurrentSupportCount; }

        var offset = _player.position - Transform.position;
        //playerとの位置差が一定以上離れたら動く
        MoveDirection =
            offset.sqrMagnitude >= GameManager.Instance.SupportMoveSqrtDistance * _supportIndex ?
            offset : Vector2.zero;
        Rb2d.velocity = MoveDirection * Speed;
    }

    public void Hit(Collider2D collision) { }
}
