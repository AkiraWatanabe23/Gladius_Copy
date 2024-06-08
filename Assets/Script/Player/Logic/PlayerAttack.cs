﻿using Constants;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAttack : PlayerSystemBase
{
    [Header("弾を射出するMuzzle")]
    [SerializeField]
    private List<Transform> _spawnMuzzles = default;
    [Header("攻撃力")]
    [SerializeField]
    private int _attackValue = 1;
    [Header("攻撃のクールタイム")]
    [SerializeField]
    private float _attackCoolTime = 1f;
    [Min(1)]
    [Range(1, 5)]
    [Header("ChargeBeam時の各倍率の攻撃力")]
    [SerializeField]
    private int[] _chargeBeamValue = default;
    [ReadOnly]
    [Tooltip("扇形の射出範囲")]
    [SerializeField]
    private Fan _fanCollider = default;
    [Header("2way弾を撃ちだすときに上下どっちに撃つか")]
    [SerializeField]
    private Diagonal _direction = Diagonal.Up;
    [Header("現在のプラスショット（確認用）")]
    [SerializeField]
    private PlusShotType _plusShotBullet = PlusShotType.None;
    [Header("初期ショット")]
    [SerializeField]
    private List<InitialBulletType> _initialBullets = default;
    [Header("使用するプラスショット")]
    [SerializeField]
    private List<PlusShotType> _plusShots = default;

    [SerializeField]
    private bool _onDrawGizmos = false;
    [Header("Gizmo表示用")]
    [Min(1f)]
    [Range(1f, 10f)]
    [SerializeField]
    private float _gizmoSquareSize = 1f;
    [Header("反射弾を撃ち出す角度")]
    [SerializeField]
    private float _reflectShotAngle = 0f;

    private bool _isAttacked = false;
    private float _attackIntervalTimer = 0f;
    private int _bulletIndex = 0;
    private List<GameObject> _bullets = default;
    private GameObject _plusShot = default;
    private GameObject _player = default;
    private bool _isCharging = false;
    private float _chargeTimer = 0f;
    private bool _isPause = false;

    public PlusShotType PlusShotBullet
    {
        get => _plusShotBullet;
        set
        {
            _plusShotBullet = value;
            _plusShot = GameManager.Instance.BulletHolder.PlusShotsDictionary[value];
        }
    }

    protected float AttackIntervalTimer
    {
        get => _attackIntervalTimer;
        private set
        {
            _attackIntervalTimer = value;
            if (_attackIntervalTimer >= _attackCoolTime)
            {
                _isAttacked = false;
                _attackIntervalTimer = 0f;
            }
        }
    }
    protected bool IsGetShootInput => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    protected bool IsGetChargeBeamInputUp => Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0);
    protected bool IsGetBulletChangeInputUp => Input.GetAxis("Mouse ScrollWheel") > 0.05f;
    protected bool IsGetBulletChangeInputDown => Input.GetAxis("Mouse ScrollWheel") < -0.05f;

    public override void Initialize(GameObject go)
    {
        _player = go;
        _bullets = new();
        foreach (var bullet in _initialBullets)
        {
            _bullets.Add(GameManager.Instance.BulletHolder.BulletsDictionary[bullet]);
        }
    }

    public override void OnUpdate()
    {
        if (_isPause) { return; }

        if (_isAttacked)
        {
            AttackIntervalTimer += Time.deltaTime;
            return;
        }

        //弾切り替え
        if (IsGetBulletChangeInputUp) { BulletChange(1); }
        if (IsGetBulletChangeInputDown) { BulletChange(-1); }
        //攻撃
        if (IsGetShootInput)
        {
            if (!_isCharging && _initialBullets[_bulletIndex] == InitialBulletType.ChargeBeam) { _isCharging = true; }
            else if (!_isAttacked) { Attack(); }
        }
        else
        {
            if (!_isCharging) { return; }

            Charge();
            //チャージ終了
            if (_isCharging && IsGetChargeBeamInputUp)
            {
                if (_chargeTimer <= 0.5f) { ChargeReset(); return; }

                ChargeBeam();
            }
        }
    }

    private void Attack()
    {
        _isAttacked = true;
        Consts.Log($"attack {_bulletIndex}");
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
                bulletData.Initialize(_attackValue, _player.layer, bullet.transform.up);
            }
            return;
        }

        if (_spawnMuzzles == null || _spawnMuzzles.Count == 0) { return; }
        for (int i = 0; i < _spawnMuzzles.Count; i++)
        {
            GameObject bullet = null;
            if (i == 0)
            {
                bullet = GameManager.Instance.ObjectPool.SpawnObject(_bullets[_bulletIndex]);
            }
            else if (i < 0 && _plusShotBullet != PlusShotType.None)
            {
                if (_plusShotBullet == PlusShotType.SupportShot) { SupportShot(); return; }

                //PlusShotを撃つ
                bullet = GameManager.Instance.ObjectPool.SpawnObject(_plusShot);
                if (_plusShotBullet == PlusShotType.TwoWay) { TwoWayBulletSetting(bullet, _spawnMuzzles[i].position); continue; }
                else if (_plusShotBullet == PlusShotType.ReflectLaser) { ReflectShot(bullet, _spawnMuzzles[i].position); continue; }
            }
            bullet.transform.position = _spawnMuzzles[i].position;
            var bulletData = bullet.GetComponent<BulletController>();
            bulletData.Initialize(_attackValue, _player.layer);
        }
    }

    private void BulletChange(int changeValue = 1)
    {
        Consts.Log("Change");
        AudioManager.Instance.PlaySE(SEType.SwitchShot);
        if (_bulletIndex + changeValue >= _bullets.Count) { _bulletIndex = 0; return; }
        if (_bulletIndex + changeValue < 0) { _bulletIndex = _bullets.Count - 1; return; }

        _bulletIndex += changeValue;
        Consts.Log($"Bullet Change {_bulletIndex}");
    }

    /// <summary> 補助兵装が弾を撃つ </summary>
    private void SupportShot()
    {
        if (GameManager.Instance.CurrentSupportCount <= 0) { return; }

        var supports = GameManager.Instance.Supports;
        foreach (var support in supports) { support.Attack(); }
    }

    private void ReflectShot(GameObject bullet, Vector3 spawnPos)
    {
        bullet.transform.position = spawnPos;

        var direction =
            spawnPos + new Vector3(MathF.Cos(_reflectShotAngle * Mathf.Deg2Rad), MathF.Sin(_reflectShotAngle * Mathf.Deg2Rad)) -
            spawnPos;

        bullet.transform.localEulerAngles = direction.normalized;

        var bulletData = bullet.GetComponent<BulletController>();
        bulletData.Initialize(_attackValue, _player.layer, bullet.transform.right);
    }

    private void TwoWayBulletSetting(GameObject bullet, Vector3 spawnPos)
    {
        bullet.transform.position = spawnPos;

        var angle = _direction == Diagonal.Up ? 45f : -45f;
        var rotation = bullet.transform.localEulerAngles;
        rotation.z = angle - 90f;
        bullet.transform.localEulerAngles = rotation;

        var bulletData = bullet.GetComponent<BulletController>();
        bulletData.Initialize(_attackValue, _player.layer, bullet.transform.up);
    }

    private void Charge()
    {
        if (_chargeTimer > _chargeBeamValue.Length) { return; }
        _chargeTimer += Time.deltaTime;
    }

    private void ChargeBeam()
    {
        Consts.Log("shot");
        var bullet = GameManager.Instance.ObjectPool.SpawnObject(
            GameManager.Instance.BulletHolder.BulletsDictionary[InitialBulletType.ChargeBeam]);
        bullet.transform.position = _spawnMuzzles[0].position;

        var bulletData = bullet.GetComponent<BulletController>();
        bulletData.Initialize(_attackValue, _player.layer, Vector2.right);
        if (bulletData.BulletData is ChargeBeamBullet)
        {
            var chargeBeamData = bulletData.BulletData as ChargeBeamBullet;
            chargeBeamData.BeamDataSetting(Mathf.FloorToInt(_chargeTimer));
        }
        ChargeReset();
    }

    private void ChargeReset()
    {
        _isCharging = false;
        _chargeTimer = 0f;
    }

    public void Pause() => _isPause = true;

    public void Resume() => _isPause = false;

    /// <summary> For Editor </summary>
    public void OnDrawGizmos(GameObject player)
    {
        if (!_onDrawGizmos || player == null) { return; }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            player.transform.position,
            player.transform.position +
            new Vector3(MathF.Cos(_reflectShotAngle * Mathf.Deg2Rad), MathF.Sin(_reflectShotAngle * Mathf.Deg2Rad)));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(player.transform.position, Vector2.one * _gizmoSquareSize);
    }
}
