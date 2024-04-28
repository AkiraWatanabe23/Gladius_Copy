using Constants;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAttack : PlayerSystemBase
{
    [Tooltip("弾を射出するMuzzle")]
    [SerializeField]
    private List<Transform> _spawnMuzzles = default;
    [SerializeField]
    private int _attackValue = 1;
    [SerializeField]
    private float _attackInterval = 1f;
    [Min(1)]
    [Range(1, 5)]
    [Tooltip("ChargeBeamの倍率上限（何倍までできるか）")]
    [SerializeField]
    private int _chargeLimit = 1;
    [Tooltip("扇形の射出範囲")]
    [SerializeField]
    private Fan _fanCollider = default;
    [Tooltip("2wayを撃ちだすときにどっちに撃つか")]
    [SerializeField]
    private Diagonal _direction = Diagonal.Up;
    [SerializeField]
    private LayerMask _playerLayer = default;
    [SerializeField]
    private PlusShotType _plusShotBullet = PlusShotType.None;
    [Tooltip("初期ショット")]
    [SerializeField]
    private List<InitialBulletType> _initialBullets = default;
    [Tooltip("使用するプラスショット")]
    [SerializeField]
    private List<PlusShotType> _plusShots = default;

    [SerializeField]
    private bool _onDrawGizmos = false;
    [Tooltip("Gizmo表示用")]
    [Min(1f)]
    [Range(1f, 10f)]
    [SerializeField]
    private float _gizmoSquareSize = 1f;

    private int _bulletIndex = 0;
    private List<GameObject> _bullets = default;
    private GameObject _plusShot = default;
    private bool _isCharging = false;
    private float _chargeTimer = 0f;
    private bool _isPause = false;

    public LayerMask Layer => _playerLayer;
    public PlusShotType PlusShotBullet
    {
        get => _plusShotBullet;
        set
        {
            _plusShotBullet = value;
            _plusShot = GameManager.Instance.BulletHolder.PlusShotsDictionary[value];
        }
    }

    protected List<GameObject> Bullets
    {
        get
        {
            if (_bullets == null)
            {
                _bullets = new();
                foreach (var bullet in _initialBullets)
                {
                    _bullets.Add(GameManager.Instance.BulletHolder.BulletsDictionary[bullet]);
                }
            }
            return _bullets;
        }
    }
    protected bool IsGetShootInput => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    protected bool IsGetChargeBeamInputUp => Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0);
    protected bool IsGetBulletChangeInputUp => Input.mouseScrollDelta.y >= 1f;
    protected bool IsGetBulletChangeInputDown => Input.mouseScrollDelta.y <= -1f;

    public override void OnUpdate()
    {
        if (_isPause) { return; }

        if (_isCharging) { Charge(); }
        //チャージ終了
        if (_isCharging && IsGetChargeBeamInputUp)
        {
            ChargeBeam();
            _isCharging = false;
        }
        //攻撃
        if (IsGetShootInput)
        {
            if (!_isCharging && _initialBullets[_bulletIndex] == InitialBulletType.ChargeBeam) { _isCharging = true; }
            else { Attack(); }
        }

        //弾切り替え
        if (IsGetBulletChangeInputUp) { BulletChange(1); }
        if (IsGetBulletChangeInputDown) { BulletChange(-1); }
    }

    private void Attack()
    {
        Consts.Log("attack");
        if (_initialBullets[_bulletIndex] == InitialBulletType.ShotGun)
        {
            var spawnBullet = GameManager.Instance.BulletHolder.BulletsDictionary[InitialBulletType.ShotGun];
            var shotCount = GameManager.Instance.ShotGunCount;
            for (int i = 0; i < shotCount; i++)
            {
                var bullet = GameManager.Instance.ObjectPool.SpawnObject(spawnBullet);
                bullet.transform.position = _spawnMuzzles[0].position;

                var randomAngle = UnityEngine.Random.Range((int)-_fanCollider.Angle, (int)_fanCollider.Angle);
                var rotation = bullet.transform.localEulerAngles;
                rotation.z = randomAngle - 90f;
                bullet.transform.localEulerAngles = rotation;

                var bulletData = bullet.GetComponent<BulletController>();
                bulletData.Initialize(1f, _attackValue, _playerLayer, bullet.transform.up);
            }
            return;
        }

        if (_spawnMuzzles == null || _spawnMuzzles.Count == 0) { return; }
        for (int i = 0; i < _spawnMuzzles.Count; i++)
        {
            GameObject bullet = null;
            if (i == 0)
            {
                bullet = GameManager.Instance.ObjectPool.SpawnObject(Bullets[_bulletIndex]);
            }
            else if (i < 0 && _plusShotBullet != PlusShotType.None)
            {
                if (_plusShotBullet == PlusShotType.SupportShot) { SupportShot(); return; }

                //PlusShotを撃つ
                bullet = GameManager.Instance.ObjectPool.SpawnObject(_plusShot);
                if (_plusShotBullet == PlusShotType.TwoWay) { TwoWayBulletSetting(bullet, _spawnMuzzles[i].position); continue; }
            }
            bullet.transform.position = _spawnMuzzles[i].position;
            var bulletData = bullet.GetComponent<BulletController>();
            bulletData.Initialize(1f, _attackValue, _playerLayer);
        }
    }

    private void BulletChange(int changeValue = 1)
    {
        if (_bulletIndex + changeValue >= Bullets.Count) { _bulletIndex = 0; return; }
        if (_bulletIndex + changeValue < 0) { _bulletIndex = Bullets.Count - 1; return; }

        _bulletIndex += changeValue;
    }

    /// <summary> 補助兵装が弾を撃つ </summary>
    private void SupportShot()
    {
        if (GameManager.Instance.CurrentSupportCount <= 0) { return; }

        var supports = GameManager.Instance.Supports;
        foreach (var support in supports) { support.Attack(); }
    }

    private void TwoWayBulletSetting(GameObject bullet, Vector3 spawnPos)
    {
        bullet.transform.position = spawnPos;

        var angle = _direction == Diagonal.Up ? 45f : -45f;
        var rotation = bullet.transform.localEulerAngles;
        rotation.z = angle - 90f;
        bullet.transform.localEulerAngles = rotation;

        var bulletData = bullet.GetComponent<BulletController>();
        bulletData.Initialize(1f, _attackValue, _playerLayer, bullet.transform.up);
    }

    private void Charge()
    {
        if (_chargeTimer > _chargeLimit) { return; }
        _chargeTimer += Time.deltaTime;
    }

    private void ChargeBeam()
    {
        Consts.Log("shot");
        var bullet = GameManager.Instance.ObjectPool.SpawnObject(
            GameManager.Instance.BulletHolder.BulletsDictionary[InitialBulletType.ChargeBeam]);
        bullet.transform.position = _spawnMuzzles[0].position;

        var bulletData = bullet.GetComponent<BulletController>();
        bulletData.Initialize(3f, _attackValue, _playerLayer, Vector2.right);
        if (bulletData.BulletData is ChargeBeamBullet)
        {
            var chargeBeamData = bulletData.BulletData as ChargeBeamBullet;
            chargeBeamData.BeamDataSetting(Mathf.FloorToInt(_chargeTimer));
        }
        _chargeTimer = 0f;
    }

    public void Pause() => _isPause = true;

    public void Resume() => _isPause = false;

    /// <summary> For Editor </summary>
    public void OnDrawGizmos(GameObject player)
    {
        if (!_onDrawGizmos || player == null) { return; }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(player.transform.position, Vector2.one * _gizmoSquareSize);
    }
}
