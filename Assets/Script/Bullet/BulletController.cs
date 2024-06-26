﻿using UnityEngine;

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

    public void Initialize(int attackValue, LayerMask gunner, Vector2 forward)
    {
        if (forward == Vector2.zero) { forward = Vector2.right; }

        _bulletData.Init(gameObject, _moveSpeed, attackValue, gunner, forward);
        if (_isChildSetting) { _bulletData.Transform.SetParent(GameManager.Instance.PlayerTransform); }

        PlayInitAudio();
    }

    private void PlayInitAudio()
    {
        var bulletSE = _bulletData switch
        {
            DefautBullet => SEType.DefaultShot,
            LaserBullet => SEType.LaserShot,
            ChargeBeamBullet => SEType.ChargeBeam,
            HomingBullet => SEType.Homing,
            ShotGun => SEType.ShotGun,
            BombBullet => SEType.ShotFire,
            MissileBullet => SEType.MissileFire,
            ReflectLaser => SEType.ReflectFire,
            Melee => SEType.MeleeMoving,
            Barrier => SEType.Barrier,
            _ => SEType.None
        };

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
