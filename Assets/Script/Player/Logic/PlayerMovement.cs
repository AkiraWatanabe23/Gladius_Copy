using System;
using UnityEngine;

[Serializable]
public class PlayerMovement : PlayerSystemBase
{
    [Header("自機の移動速度")]
    [Range(1f, 15f)]
    [SerializeField]
    private float _moveSpeed = 5f;
    [Header("自機の移動最低速度")]
    [Range(0.1f, 14f)]
    [SerializeField]
    private float _minSpeed = 1f;

    private Rigidbody2D _rb2d = default;
    private bool _isPause = false;

    public override void Initialize(GameObject go)
    {
        if (!go.TryGetComponent(out _rb2d)) { _rb2d = go.AddComponent<Rigidbody2D>(); }
        _rb2d.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void OnUpdate()
    {
        if (_isPause) { return; }
        Movement();
    }

    private void Movement()
    {
        if (_rb2d == null) { Debug.Log("Rigidbody2D is not assigned"); return; }

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        _rb2d.velocity = new Vector2(horizontal, vertical) * _moveSpeed;
    }

    public void SpeedUp(float value)
    {
        _moveSpeed += value;
    }

    public void SpeedDown(float value)
    {
        _moveSpeed -= value;
        //最低値は割らないようにする
        if (_moveSpeed <= _minSpeed) { _moveSpeed = _minSpeed; }
    }

    public void Pause() => _isPause = true;

    public void Resume() => _isPause = false;
}
