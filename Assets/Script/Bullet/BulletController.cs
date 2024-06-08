using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Range(1f, 40f)]
    [SerializeField]
    private float _moveSpeed = 1f;
    [Tooltip("Playerの子オブジェクトに設定するか")]
    [SerializeField]
    private bool _isChildSetting = false;
    [SubclassSelector]
    [SerializeReference]
    private IBulletData _bulletData = default;

    public IBulletData BulletData => _bulletData;

    public void Initialize(int attackValue, LayerMask gunner)
    {
        _bulletData.Init(gameObject, _moveSpeed, attackValue, gunner, Vector2.right);
        if (_isChildSetting) { _bulletData.Transform.SetParent(GameManager.Instance.PlayerTransform); }

        PlayInitAudio();
    }

    public void Initialize(int attackValue, LayerMask gunner, Vector2 initialDirection)
    {
        _bulletData.Init(gameObject, _moveSpeed, attackValue, gunner, initialDirection);
        if (_isChildSetting) { _bulletData.Transform.SetParent(GameManager.Instance.PlayerTransform); }

        PlayInitAudio();
    }

    private void PlayInitAudio()
    {
        var bulletSE = SEType.None;
        if (_bulletData is DefautBullet) { bulletSE = SEType.DefaultShot; }
        else if (_bulletData is LaserBullet) { bulletSE = SEType.LaserShot; }
        else if (_bulletData is ChargeBeamBullet) { bulletSE = SEType.ChargeBeam; }
        else if (_bulletData is HomingBullet) { bulletSE = SEType.Homing; }
        else if (_bulletData is ShotGun) { bulletSE = SEType.ShotGun; }
        else if (_bulletData is BombBullet) { bulletSE = SEType.ShotFire; }
        else if (_bulletData is MissileBullet) { bulletSE = SEType.MissileFire; }
        else if (_bulletData is ReflectLaser) { bulletSE = SEType.ReflectFire; }
        else if (_bulletData is Melee) { bulletSE = SEType.MeleeMoving; }
        else if (_bulletData is Barrier) { bulletSE = SEType.Barrier; }

        AudioManager.Instance.PlaySE(bulletSE);
    }

    private void Update()
    {
        _bulletData.Movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //衝突対象が攻撃対象でなければ無視
        if (collision.gameObject == null) { return; }
        if (collision.gameObject.TryGetComponent(out Fan _)) { return; }
        if (collision.gameObject.layer == _bulletData.GunnerLayer) { return; }

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
