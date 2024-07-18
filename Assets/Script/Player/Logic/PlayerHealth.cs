using Constants;
using System;
using UnityEngine;

#region Serialized Data
public interface IHealth : ISerializableParam
{
    public HealthType HealthType { get; }
}

public class HP : IHealth
{
    [Header("現在の体力（確認用）")]
    [ReadOnly]
    [SerializeField]
    private int _currentLife = 1;
    [Header("体力の最大値")]
    [SerializeField]
    private int _maxLife = 1;

    public int Life => _currentLife;
    public int MaxLife => _maxLife;
    public HealthType HealthType => HealthType.HP;

    public void SettingLife(int maxLife) => _currentLife = maxLife;
}

public class RemainingAircraft : IHealth
{
    [Header("残機数")]
    [ReadOnly]
    [SerializeField]
    private int _remainingAircraft = 5;
    [Header("最大の残機数")]
    [SerializeField]
    private int _maxRemainingAircraft = 5;

    public int RemainingAircraftCount => _remainingAircraft;
    public int MaxRemainingAircraft => _maxRemainingAircraft;
    public HealthType HealthType => HealthType.RemainingAircraft;

    public void SettingRemainingAircraft(int count) => _remainingAircraft = count;
}
#endregion

[Serializable]
public class PlayerHealth :  PlayerSystemBase
{
    [SubclassSelector]
    [SerializeReference]
    private IHealth _healthData = default;

    private HP _hpInstance = default;
    private RemainingAircraft _aircraftInstance = default;

    public HP HP => _hpInstance;
    public RemainingAircraft Aircraft => _aircraftInstance;
    public HealthType HealthType => _healthData.HealthType;

    public override void Initialize(GameObject go)
    {
        if (_healthData.HealthType == HealthType.HP)
        {
            _hpInstance = _healthData as HP;
            _hpInstance.SettingLife(_hpInstance.MaxLife);
        }
        else if (_healthData.HealthType == HealthType.RemainingAircraft)
        {
            _aircraftInstance = _healthData as RemainingAircraft;
            if (_aircraftInstance.RemainingAircraftCount <= 0) { _aircraftInstance.SettingRemainingAircraft(1); }
        }
    }

    public void ReceiveDamage(int value)
    {
        AudioManager.Instance.PlaySE(SEType.PlayerDamaged);
        if (_healthData.HealthType == HealthType.HP)
        {
            _hpInstance.SettingLife(_hpInstance.Life - value);
            GameManager.Instance.UIController.OnUpdateHealth($"{_hpInstance.Life} / {_hpInstance.MaxLife}");

            if (_hpInstance.Life <= 0) { Dead(); }
        }
        else if (_healthData.HealthType == HealthType.RemainingAircraft)
        {
            _aircraftInstance.SettingRemainingAircraft(_aircraftInstance.RemainingAircraftCount - 1);
            GameManager.Instance.UIController.OnUpdateAircraft();
            if (_aircraftInstance.RemainingAircraftCount <= 0) { Dead(); }
        }
    }

    public void Heal(int value)
    {
        if (_healthData.HealthType == HealthType.HP)
        {
            _hpInstance.SettingLife(_hpInstance.Life + value);
            if (_hpInstance.Life > _hpInstance.MaxLife) { _hpInstance.SettingLife(_hpInstance.MaxLife); }

            GameManager.Instance.UIController.OnUpdateHealth($"{_hpInstance.Life} / {_hpInstance.MaxLife}");
        }
        else if (_healthData.HealthType == HealthType.RemainingAircraft)
        {
            _aircraftInstance.SettingRemainingAircraft(_aircraftInstance.RemainingAircraftCount + 1);
            if (_aircraftInstance.RemainingAircraftCount > _aircraftInstance.MaxRemainingAircraft)
            {
                _aircraftInstance.SettingRemainingAircraft(_aircraftInstance.MaxRemainingAircraft);
            }
            GameManager.Instance.UIController.OnUpdateAircraft(_aircraftInstance.MaxRemainingAircraft, true);
        }
    }

    private void Dead()
    {
        if (_healthData.HealthType == HealthType.HP)
        {
            _hpInstance.SettingLife(0);
        }
        else if (_healthData.HealthType == HealthType.RemainingAircraft)
        {
            _aircraftInstance.SettingRemainingAircraft(0);
        }
        Consts.Log("GameOver");
        AudioManager.Instance.PlaySE(SEType.PlayerCrashed);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground _)) { Dead(); }
    }
}

public enum HealthType
{
    HP,
    RemainingAircraft,
}
