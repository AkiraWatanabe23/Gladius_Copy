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
    [ReadOnly]
    [SerializeField]
    private int _currentLife = 1;
    [SerializeField]
    private int _maxLife = 1;

    public int Life => _currentLife;
    public int MaxLife => _maxLife;
    public HealthType HealthType => HealthType.HP;

    public void SettingLife(int maxLife) => _currentLife = maxLife;
}

public class RemainingAircraft : IHealth
{
    [Tooltip("残機数")]
    [ReadOnly]
    [SerializeField]
    private int _remainingAircraft = 5;
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
        if (_healthData.HealthType == HealthType.HP)
        {
            _hpInstance.SettingLife(_hpInstance.Life - value);
            if (_hpInstance.Life <= 0) { Consts.Log("GameOver"); }
        }
        else if (_healthData.HealthType == HealthType.RemainingAircraft)
        {
            _aircraftInstance.SettingRemainingAircraft(_aircraftInstance.RemainingAircraftCount - 1);
            if (_aircraftInstance.RemainingAircraftCount <= 0) { Consts.Log("GameOver"); }
        }
    }

    public void Heal(int value)
    {
        if (_healthData.HealthType == HealthType.HP)
        {
            _hpInstance.SettingLife(_hpInstance.Life + value);
            if (_hpInstance.Life > _hpInstance.MaxLife) { _hpInstance.SettingLife(_hpInstance.MaxLife); }
        }
        else if (_healthData.HealthType == HealthType.RemainingAircraft)
        {
            _aircraftInstance.SettingRemainingAircraft(_aircraftInstance.RemainingAircraftCount + 1);
            if (_aircraftInstance.RemainingAircraftCount > _aircraftInstance.MaxRemainingAircraft)
            {
                _aircraftInstance.SettingRemainingAircraft(_aircraftInstance.MaxRemainingAircraft);
            }
        }
    }
}

public enum HealthType
{
    HP,
    RemainingAircraft,
}
