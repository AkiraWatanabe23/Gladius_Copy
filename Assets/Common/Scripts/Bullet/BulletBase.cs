using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

// 日本語対応
public class BulletBase : MonoBehaviour, IBulletData
{
    public float Speed { get => _speed; set => _speed = value; }
    public int Damage { get => _damage; set => _damage = value; }
    /// <summary>Colliderに接触したかどうか</summary>
    public bool IsTriggerEnter { get; private set; } = false;
    /// <summary>Colliderに接触しているかどうか</summary>
    public bool IsTriggerStay { get; private set; } = false;
    /// <summary>接触していたColliderから出たかどうか</summary>
    public bool IsTriggerExit { get; private set; } = false;

    /// <summary>Colliderに接触したときに実行するイベント</summary>
    protected event Action<GameObject> OnTriggerEnterEvent { add => _onTriggerEnter += value; remove => _onTriggerEnter -= value; }
    /// <summary>Colliderに接触しているあいだ実行するイベント</summary>
    protected event Action<GameObject> OnTriggerStayEvent { add => _onTriggerStay += value; remove => _onTriggerStay -= value; }
    /// <summary>接触していたColliderから出たときに実行するイベント</summary>
    protected event Action<GameObject> OnTriggerExitEvent { add => _onTriggerExit += value; remove => _onTriggerExit -= value; }

    protected Rigidbody2D Rigidbody2D => _rigidbody2D;

    [SerializeField] private float _speed = 0.0f;
    [SerializeField] private int _damage = 0;
    [SerializeField] private Rigidbody2D _rigidbody2D = null;

    private event Action<GameObject> _onTriggerEnter = null;
    private event Action<GameObject> _onTriggerStay = null;
    private event Action<GameObject> _onTriggerExit = null;

    private void Start()
    {
        #region Initialize Regidbody
        if (!_rigidbody2D)
        {
            _rigidbody2D = TryGetComponent(out Rigidbody2D rb2d) switch
            {
                true => rb2d,
                false => gameObject.AddComponent<Rigidbody2D>(),
            };
        }
        _rigidbody2D.isKinematic = true;
        #endregion

        switch (TryGetComponent(out Collider2D collider))
        {
            case true:
                collider.isTrigger = true; break;
            case false:
                Debug.Log("Collider2D is not found"); break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _onTriggerEnter?.Invoke(collision.gameObject);
        DetectionTriggerEnterAsync();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _onTriggerStay?.Invoke(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _onTriggerExit?.Invoke(collision.gameObject);
        DetectionTriggerExitAsync();
    }

    private async void DetectionTriggerEnterAsync()
    {
        IsTriggerStay = true;
        IsTriggerEnter = true;
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
