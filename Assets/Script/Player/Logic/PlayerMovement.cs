using System;
using UnityEngine;

[Serializable]
public class PlayerMovement : PlayerSystemBase
{
    [Range(1f, 10f)]
    [SerializeField]
    private float _moveSpeed = 5f;
    [Tooltip("自機の移動最低速度（初期値からどれくらいまで下げるか\n例：0.5 -> MoveSpeed - 0.5が自機の最低移動速度）")]
    [Range(0.1f, 10f)]
    [SerializeField]
    private float _minSpeedValue = 1f;

    private Rigidbody2D _rb2d = default;
    /// <summary> 自機の移動速度の最低値 </summary>
    private float _minSpeed = 1f;

    public override void Initialize(GameObject go)
    {
        if (!go.TryGetComponent(out _rb2d)) { _rb2d = go.AddComponent<Rigidbody2D>(); }
        _rb2d.bodyType = RigidbodyType2D.Kinematic;

        _minSpeed = _moveSpeed - _minSpeedValue;
    }

    public override void OnUpdate() { Movement(); }

    private void Movement()
    {
        if (_rb2d == null) { Debug.Log("Rigidbody2D is not assigned"); return; }

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        _rb2d.velocity = new Vector2(horizontal, vertical) * _moveSpeed;
    }

    public void SpeedUp(float value)
    {
        _moveSpeed *= value;
    }

    public void SpeedDown(float value)
    {
        _moveSpeed -= value;
        //最低値は割らないようにする
        if (_moveSpeed <= _minSpeed) { _moveSpeed = _minSpeed; }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //ここに移動による当たり判定の記述をする
        if (collision.gameObject.TryGetComponent(out ItemController item))
        {
            item.ItemSystem.PlayEffect();
            GameManager.Instance.ObjectPool.RemoveObject(collision.gameObject);
        }
    }
}
