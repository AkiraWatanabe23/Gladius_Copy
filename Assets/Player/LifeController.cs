using System;
using UnityEngine;

[Serializable]
// 日本語対応
public class LifeController
{
    public int MaxLife => _maxLife;
    public int CurrentLife => _curLife;
    public bool IsAlive => _curLife > 0.0f;

    public event Action OnDead { add => _onDead += value; remove => _onDead -= value; }

    [SerializeField] private int _maxLife = 0;
    [SerializeField] private int _curLife = 0;

    private event Action _onDead = null;

    public LifeController(int initializeValue)
    {
        _maxLife = initializeValue;
        _curLife = initializeValue;
    }

    public void Damage(int damage)
    {
        _curLife = Mathf.Clamp(_curLife - damage, 0, _maxLife);

        if (!IsAlive)
        {
            _onDead?.Invoke();
        }
    }

    public void Recover(int recover)
    {
        _curLife = Mathf.Clamp(_curLife + recover, 0, _maxLife);
    }
}
