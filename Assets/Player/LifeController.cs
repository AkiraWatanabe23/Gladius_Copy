using System;
using UnityEngine;

[Serializable]
// 日本語対応
public class LifeController
{
    public float MaxLife => _maxLife;
    public float CurrentLife => _curLife;
    public bool IsAlive => _curLife > 0.0f;

    [SerializeField] private float _maxLife = 0.0f;
    [SerializeField] private float _curLife = 0.0f;

    public LifeController(float initializeValue)
    {
        _maxLife = initializeValue;
        _curLife = initializeValue;
    }

    public void Damage(float damage)
    {
        _curLife = Mathf.Clamp(_curLife - damage, 0.0f, _maxLife);
    }

    public void Recover(float recover)
    {
        _curLife = Mathf.Clamp(_curLife + recover, 0.0f, _maxLife);
    }
}
