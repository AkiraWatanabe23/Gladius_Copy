using UnityEngine;

// 日本語対応
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerInput _input = new();
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private int _maxLife = 1;
    [SerializeField] private BulletBase _bullet = null;

    [SerializeField] private BulletController _bulletPrefab = default;

    private Rigidbody2D _rb2d = null;
    private LifeController _life = null;

    private void Start()
    {
        if (!TryGetComponent(out _rb2d)) { _rb2d = gameObject.AddComponent<Rigidbody2D>(); }
        _rb2d.bodyType = RigidbodyType2D.Kinematic;

        _life = new LifeController(_maxLife);
        _life.OnDead += Destroy;
    }

    private void Update()
    {
        Input2Velocity();
        Shoot();
    }

    private void OnDisable()
    {
        _life.OnDead -= Destroy;
    }

    public void ReceiveDamage(int value)
    {
        _life.Damage(value);
    }

    private void Input2Velocity()
    {
        Vector2 velocity = new Vector2(_input.Horizontal, _input.Vertical).normalized * _moveSpeed;
        _rb2d.velocity = velocity;
    }

    private void Shoot()
    {
        if (!_bullet) { return; }

        if (_input.IsShoot)
        {
            var bullet = Instantiate(_bullet, transform.position, _bullet.transform.rotation);
            bullet.Init(gameObject.layer);
            Debug.Log("Shoot!");
        }
    }

    private void Destroy()
    {
        Debug.Log("My life is over");
        Destroy(gameObject);
    }
}
