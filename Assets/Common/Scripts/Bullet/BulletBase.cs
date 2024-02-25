using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

// 日本語対応
public class BulletBase : MonoBehaviour, IBulletData
{
    public float Speed { get => _initSpeed; set => _initSpeed = value; }
    public int Damage { get => _initDamage; set => _initDamage = value; }
    /// <summary>Colliderに接触したかどうか</summary>
    public bool IsTriggerEnter { get; private set; } = false;
    /// <summary>Colliderに接触しているかどうか</summary>
    public bool IsTriggerStay { get; private set; } = false;
    /// <summary>接触していたColliderから出たかどうか</summary>
    public bool IsTriggerExit { get; private set; } = false;

    protected Rigidbody2D Rigidbody2D => _rigidbody2D;
    /// <summary>Colliderに接触したときに実行するイベント</summary>
    protected event Action<GameObject> OnTriggerEnterEvent;
    /// <summary>Colliderに接触しているあいだ実行するイベント</summary>
    protected event Action<GameObject> OnTriggerStayEvent;
    /// <summary>接触していたColliderから出たときに実行するイベント</summary>
    protected event Action<GameObject> OnTriggerExitEvent;

    [SerializeField] private float _initSpeed = 1.0f;
    [SerializeField] private int _initDamage = 0;
    [SerializeField] private Rigidbody2D _rigidbody2D = null;

    /// <summary>自身を撃ったオブジェクトのレイヤー</summary>
    private LayerMask _gunnerLayer = 0;
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
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
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
        if (collision.gameObject.layer == _gunnerLayer) return;

        _onTriggerEnter?.Invoke(collision.gameObject);
        DetectionTriggerEnter();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _gunnerLayer) return;

        _onTriggerStay?.Invoke(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _gunnerLayer) return;

        _onTriggerExit?.Invoke(collision.gameObject);
        DetectionTriggerExit();
    }

    private async void DetectionTriggerEnter()
    {
        IsTriggerStay = true;
        IsTriggerEnter = true;
        await UniTask.DelayFrame(1);
        IsTriggerEnter = false;
    }

    private async void DetectionTriggerExit()
    {
        IsTriggerStay = false;
        IsTriggerExit = true;
        await UniTask.DelayFrame(1);
        IsTriggerExit = false;
    }

    public virtual void Init(LayerMask layer)
    {
        _gunnerLayer = layer;
    }
}
