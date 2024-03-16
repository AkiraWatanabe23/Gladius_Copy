using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SubclassSelector]
    [SerializeReference]
    private IBulletData _bulletData = default;

    public void Initialize(float speed, int attackValue, LayerMask gunner, Vector2 direction)
    {
        _bulletData.Init(gameObject, speed, attackValue, gunner, direction);
    }

    private void Update()
    {
        _bulletData.Movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //衝突対象が攻撃対象でなければ無視
        if (collision.gameObject.layer == _bulletData.GunnerLayer) { return; }

        _bulletData.Hit(collision);
    }

    private void OnBecameInvisible()
    {
        EnemyManager.Instance.EnemyCommon.ObjectPool.RemoveObject(gameObject);
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

public enum BulletType
{
    Default,
    /// <summary> 貫通 </summary>
    Penetration,
    Missile,
    /// <summary> 追尾 </summary>
    Tracking,
    Bomb
}
