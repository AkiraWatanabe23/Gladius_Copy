using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

// 日本語対応
public class BulletBase : MonoBehaviour, IBulletData
{
    public float Speed { get => _speed; set => _speed = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public bool IsTriggerEnter { get; private set; } = false;
    public bool IsTriggerStay { get; private set; } = false;
    public bool IsTriggerExit { get; private set; } = false;

    public event Action<Collider2D> OnTriggerEnterEvent { add => _onTriggerEnter += value; remove => _onTriggerEnter -= value; }
    public event Action<Collider2D> OnTriggerStayEvent { add => _onTriggerStay += value; remove => _onTriggerStay -= value; }
    public event Action<Collider2D> OnTriggerExitEvent { add => _onTriggerExit += value; remove => _onTriggerExit -= value; }

    protected Rigidbody2D Rigidbody2D => _rigidbody2D;

    [SerializeField] private float _speed = 0.0f;
    [SerializeField] private int _damage = 0;
    [SerializeField] private Rigidbody2D _rigidbody2D = null;

    private event Action<Collider2D> _onTriggerEnter = null;
    private event Action<Collider2D> _onTriggerStay = null;
    private event Action<Collider2D> _onTriggerExit = null;

    private void Start()
    {
        RigidbodyInitialize();

        switch (TryGetComponent(out Collider2D collider))
        {
            case true:
                collider.isTrigger = true; break;
            case false:
                Debug.Log("Collider2D is not found"); break;
        }
    }

    private void RigidbodyInitialize()
    {
        if (!_rigidbody2D)
            _rigidbody2D = TryGetComponent(out Rigidbody2D rb2d) switch
            {
                true => rb2d,
                false => gameObject.AddComponent<Rigidbody2D>(),
            };
        _rigidbody2D.isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _onTriggerEnter?.Invoke(collision);
        DetectionTriggerEnterAsync();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _onTriggerStay?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _onTriggerExit?.Invoke(collision);
        DetectionTriggerExitAsync();
    }

    private async void DetectionTriggerEnterAsync()
    {
        IsTriggerEnter = true;
        IsTriggerStay = true;
        await UniTask.DelayFrame(1);
        IsTriggerEnter = false;
    }

    private async void DetectionTriggerExitAsync()
    {
        IsTriggerStay = false;
        IsTriggerExit = true;
        await UniTask.DelayFrame(1);
        IsTriggerExit = false;
    }
}
