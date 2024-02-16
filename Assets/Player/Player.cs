using UnityEngine;

// 日本語対応
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput _input = new PlayerInput();
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private Rigidbody2D _rb2d = null;

    private void Update()
    {
        Vector2 velocity = new Vector2(_input.Horizontal, _input.Vertical).normalized * _moveSpeed;
        _rb2d.velocity = velocity;
    }
}
