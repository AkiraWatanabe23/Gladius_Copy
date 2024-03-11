using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SubclassSelector]
    [SerializeReference]
    private IBulletData _bulletData = default;

    public void Intialize(float speed, int attackValue, LayerMask gunner, Vector2 direction)
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
