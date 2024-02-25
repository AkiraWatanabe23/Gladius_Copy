using UnityEngine;

// 日本語対応
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerInput _input = new PlayerInput();
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private int _maxLife = 1;
    [SerializeField] private Rigidbody2D _rb2d = null;
    [SerializeField] private BulletBase _bullet = null;

    private LifeController _life = null;

    private void Start()
    {
        if (_rb2d)
        {
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
        else throw new System.NullReferenceException($"{_rb2d} is not found");

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
        if (!_bullet) return;

        if (_input.IsShoot)
        {
            var bullet = Instantiate(_bullet, transform.position, _bullet.transform.rotation);
            bullet.Init(gameObject.layer);
            Debug.Log("Shoot!");
        }
        else
        {

        }
    }

    private void Destroy()
    {
        Debug.Log("My life is over");
        Destroy(gameObject);
    }
}
