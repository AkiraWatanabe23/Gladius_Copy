using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable]
public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] private float _initSpeed = 1.0f;
    [SerializeField] private int _initDamage = 0;
    [SerializeField] private float _coolTime = 1.0f;

    private Rigidbody2D _rigidbody2D = null;
    /// <summary>自身を撃ったオブジェクトのレイヤー</summary>
    private LayerMask _gunnerLayer = 0;

    protected Rigidbody2D Rigidbody2D => _rigidbody2D;

    protected event Action OnAwakeEvent = null;
    protected event Action OnEnableEvent = null;
    protected event Action OnStartEvent = null;
    protected event Action OnFixedUpdateEvent = null;
    protected event Action OnUpdateEvent = null;
    protected event Action OnLateUpdateEvent = null;
    protected event Action OnDisableEvent = null;
    protected event Action OnDestroyEvent = null;
    protected event Action<GameObject> OnTriggerEnterEvent = null;
    protected event Action<GameObject> OnTriggerStayEvent = null;
    protected event Action<GameObject> OnTriggerExitEvent = null;

    protected abstract void BaseEventRegister();
    protected abstract void BaseEventUnregister();

    public float Speed
    {
        get => _rigidbody2D.velocity.magnitude;
        set => _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * Mathf.Max(value, 0.0f);
    }
    public Vector2 Direction { get => _rigidbody2D.velocity.normalized; set => _rigidbody2D.velocity = value * Speed; }
    public int AttackValue { get => _initDamage; set => _initDamage = Mathf.Max(value, 0); }
    public float CoolTime { get => _coolTime; set => _coolTime = Mathf.Max(value, 0.0f); }
    /// <summary>Colliderに接触したかどうか</summary>
    public bool IsTriggerEnter { get; private set; } = false;
    /// <summary>Colliderに接触しているかどうか</summary>
    public bool IsTriggerStay { get; private set; } = false;
    /// <summary>接触していたColliderから出たかどうか</summary>
    public bool IsTriggerExit { get; private set; } = false;

    public virtual void Init(LayerMask layer)
    {
        _gunnerLayer = layer;
    }

    #region General LifeCycle
    private void Awake()
    {
        BaseEventRegister();
        OnAwakeEvent?.Invoke();
    }
    private void OnEnable()
    {
        OnEnableEvent?.Invoke();
    }
    private void Start()
    {
        BaseInit();
        OnStartEvent?.Invoke();
    }
    private void FixedUpdate()
    {
        OnFixedUpdateEvent?.Invoke();
    }
    private void Update()
    {
        OnUpdateEvent?.Invoke();
    }
    private void LateUpdate()
    {
        OnLateUpdateEvent?.Invoke();
    }
    private void OnDisable()
    {
        OnDisableEvent?.Invoke();
    }
    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
        BaseEventUnregister();
    }
    #endregion

    #region OnTriggerEvent
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _gunnerLayer) return;

        OnTriggerEnterEvent?.Invoke(collision.gameObject);
        DetectionTriggerEnter();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _gunnerLayer) return;

        OnTriggerStayEvent?.Invoke(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _gunnerLayer) return;

        OnTriggerExitEvent?.Invoke(collision.gameObject);
        DetectionTriggerExit();
    }
    /// <summary>Colliderの接触を検知したときに呼び出す</summary>
    private async void DetectionTriggerEnter()
    {
        IsTriggerStay = true;
        IsTriggerEnter = true;
        await UniTask.DelayFrame(1);
        IsTriggerEnter = false;
    }
    /// <summary>接触していたColliderから出たときに呼び出す</summary>
    private async void DetectionTriggerExit()
    {
        IsTriggerStay = false;
        IsTriggerExit = true;
        await UniTask.DelayFrame(1);
        IsTriggerExit = false;
    }
    #endregion

    private void BaseInit()
    {
        if (!_rigidbody2D)
        {
            _rigidbody2D = TryGetComponent(out Rigidbody2D rb2d) switch
            {
                true => rb2d,
                false => gameObject.AddComponent<Rigidbody2D>(),
            };
        }
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        switch (TryGetComponent(out Collider2D collider))
        {
            case true:
                collider.isTrigger = true; break;
            case false:
                Debug.Log("Collider2D is not found"); break;
        }
    }
}
