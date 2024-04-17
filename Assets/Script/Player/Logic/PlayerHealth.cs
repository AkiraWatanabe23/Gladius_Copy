using Constants;
using System;
using UnityEngine;

[Serializable]
public class PlayerHealth :  PlayerSystemBase
{
    [SerializeField]
    private HealthType _healthType = HealthType.HP;
    [Header("HP制の場合")]
    [ReadOnly]
    [SerializeField]
    private int _currentLife = 1;
    [SerializeField]
    private int _maxLife = 1;
    [Header("残規制の場合")]
    [Tooltip("残機数")]
    [SerializeField]
    private int _remainingAircraft = 5;
    [SerializeField]
    private int _maxRemainingAircraft = 5;

    public HealthType HealthType => _healthType;
    public int Life => _currentLife;
    public int MaxLife => _maxLife;
    public int RemainingAircraft => _remainingAircraft;
    public int MaxRemainingAircraft => _maxRemainingAircraft;

    public override void Initialize(GameObject go)
    {
        if (_healthType == HealthType.HP) { _currentLife = _maxLife; }
        else if (_healthType == HealthType.RemainingAircraft)
        {
            //最低1の残機を設定する
            if (_remainingAircraft <= 0) { _remainingAircraft = 1; }
        }
    }

    public void ReceiveDamage(int value)
    {
        if (_healthType == HealthType.HP)
        {
            _currentLife -= value;
            if (_currentLife <= 0)
            {
                Consts.Log("GameOver");
            }
        }
        else if (_healthType == HealthType.RemainingAircraft)
        {
            _remainingAircraft--;
            if (_remainingAircraft <= 0) //GameOver
            {
                Consts.Log("GameOver");
            }
        }
    }

    public void Heal(int value)
    {
        if (_healthType == HealthType.HP)
        {
            _currentLife += value;
            if (_currentLife > _maxLife) { _currentLife = _maxLife; }
        }
        else if (_healthType == HealthType.RemainingAircraft)
        {
            _remainingAircraft++;
            if (_remainingAircraft > _maxRemainingAircraft) { _remainingAircraft = _maxRemainingAircraft; }
        }
    }
}

public enum HealthType
{
    HP,
    RemainingAircraft,
}
