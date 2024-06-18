using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class InitialBullet
{
    [field: SerializeField]
    public InitialBulletType BulletType { get; private set; }
    [field: SerializeField]
    public GameObject BulletPrefab { get; private set; }
}

[Serializable]
public class PlusShot
{
    [field: SerializeField]
    public PlusShotType ShotType { get; private set; }
    [field: SerializeField]
    public GameObject BulletPrefab { get; private set; }
}

[Serializable]
public class BulletHolder
{
    [Tooltip("初期ショットのList")]
    [SerializeField]
    private List<InitialBullet> _initialBullets = default;
    [Tooltip("プラスショットのList")]
    [SerializeField]
    private List<PlusShot> _plusShots = default;
    [SerializeField]
    private GameObject _enemyBullet = default;

    private Dictionary<InitialBulletType, GameObject> _bulletsDictionary = default;
    private Dictionary<PlusShotType, GameObject> _plusShotsDictionary = default;

    public Dictionary<InitialBulletType, GameObject> BulletsDictionary
    {
        get
        {
            if (_bulletsDictionary == null)
            {
                _bulletsDictionary = new();
                foreach (var bullet in _initialBullets) { _bulletsDictionary.Add(bullet.BulletType, bullet.BulletPrefab); }
            }
            return _bulletsDictionary;
        }
    }
    public Dictionary<PlusShotType, GameObject> PlusShotsDictionary
    {
        get
        {
            if (_plusShotsDictionary == null)
            {
                _plusShotsDictionary = new();
                foreach (var shot in _plusShots) { _plusShotsDictionary.Add(shot.ShotType, shot.BulletPrefab); }
            }
            return _plusShotsDictionary;
        }
    }
    public GameObject EnemyBullet => _enemyBullet;
}

public interface IBulletData
{
    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    /// <summary> 自身を撃ったオブジェクトのLayer </summary>
    public int GunnerLayer { get; set; }
    public Vector2 MoveForward { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    /// <summary> 初期化関数 </summary>
    public void Init(GameObject go, float speed, int attackValue, LayerMask gunner, Vector2 forward)
    {
        BulletObj = go;
        Transform = BulletObj.transform;
        Transform.forward = forward;
        Speed = speed;
        AttackValue = attackValue;
        GunnerLayer = gunner;
        MoveForward = forward;

        if (go.TryGetComponent(out Rigidbody2D rb2d)) { Rb2d = rb2d; }
        else { Rb2d = go.AddComponent<Rigidbody2D>(); }

        Rb2d.isKinematic = true;
        Rb2d.gravityScale = 0f;
    }

    public void Movement();

    /// <summary> オブジェクトとの衝突時に実行される関数 </summary>
    public void Hit(Collider2D collision) { }
}

/// <summary> 初期ショットに設定可能な弾の種類 </summary>
public enum InitialBulletType
{
    None,
    Default,
    Laser,
    ChargeBeam,
    Homing,
    ShotGun,
    Bomb
}

/// <summary> アイテムで追加することができる弾の種類 </summary>
public enum PlusShotType
{
    None,
    Missile,
    TwoWay,
    SupportShot,
    ReflectLaser,
    Melee,
    Barrier
}
