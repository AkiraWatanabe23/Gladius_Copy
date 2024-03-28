using System;
using UnityEngine;

[Serializable]
public class PlayerMovement : PlayerSystemBase
{
    [SerializeField]
    private float _moveSpeed = 5f;

    private Rigidbody2D _rb2d = default;

    public float MoveSpeed { get => _moveSpeed; private set => _moveSpeed = value; }

    public override void Initialize(GameObject go)
    {
        if (!go.TryGetComponent(out _rb2d)) { _rb2d = go.AddComponent<Rigidbody2D>(); }
        _rb2d.bodyType = RigidbodyType2D.Kinematic;
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //ここに移動による当たり判定の記述をする
        if (collision.gameObject.TryGetComponent(out ItemController item))
        {
            item.ItemSystem.PlayEffect();
        }
    }
}
