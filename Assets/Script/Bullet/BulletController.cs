using UnityEngine;

public interface IInitialBullet : ISerializableParam
{
    public InitialBulletType BulletType { get; }
}

public interface IPlusShot : ISerializableParam
{
    public PlusShotType PlusShotType { get; }
}

public class BulletController : MonoBehaviour
{
    [Tooltip("Playerの子オブジェクトに設定するか")]
    [SerializeField]
    private bool _isChildSetting = false;
    [SerializeField]
    private Vector2 _initialDirection = Vector2.zero;
    [SubclassSelector]
    [SerializeReference]
    private IBulletData _bulletData = default;

    public IBulletData BulletData => _bulletData;

    public void Initialize(float speed, int attackValue, LayerMask gunner)
    {
        _bulletData.Init(gameObject, speed, attackValue, gunner, _initialDirection);
        if (_isChildSetting) { _bulletData.Transform.SetParent(GameManager.Instance.PlayerTransform); }
    }

    public void Initialize(float speed, int attackValue, LayerMask gunner, Vector2 initialDirection)
    {
        _bulletData.Init(gameObject, speed, attackValue, gunner, initialDirection);
        if (_isChildSetting) { _bulletData.Transform.SetParent(GameManager.Instance.PlayerTransform); }
    }

    private void Update()
    {
        _bulletData.Movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //衝突対象が攻撃対象でなければ無視
        if (collision.gameObject.layer == _bulletData.GunnerLayer) { return; }
        if (collision.gameObject == null) { return; }

        _bulletData.Hit(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_bulletData is ShotGun && collision.gameObject.TryGetComponent(out Fan _))
        {
            var shotGun = _bulletData as ShotGun;
            shotGun.AreaExit();
        }
        else if (collision.gameObject.TryGetComponent(out VolcanicBomb _))
        {
            GameManager.Instance.ObjectPool.RemoveObject(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        GameManager.Instance.ObjectPool.RemoveObject(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_bulletData is not BombBullet) { return; }

        var bomb = (BombBullet)_bulletData;

        var old = Gizmos.color;
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(gameObject.transform.position, bomb.BombAreaRadius);
        Gizmos.color = old;
    }
#endif
}
