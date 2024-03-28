using System;
using UnityEngine;

[Serializable]
public class PlayerHealth :  PlayerSystemBase
{
    [SerializeField]
    private int _life = 1;
    [SerializeField]
    private int _maxLife = 1;
    [Tooltip("残機数")]
    [SerializeField]
    private int _remainingAircraft = 5;

    public int Life => _life;
    public int MaxLife => _maxLife;

    public override void Initialize(GameObject go)
    {
        _life = _maxLife;
        //最低1の残機を設定する
        if (_remainingAircraft <= 0) { _remainingAircraft = 1; }
    }

    public void ReceiveDamage(int value)
    {
        _life -= value;
        if (_life <= 0)
        {
            _remainingAircraft--;
            if (_remainingAircraft <= 0) //GameOver
            {
                Debug.Log("GameOver");
            }
            else //残機が減ってやり直し
            {
                _life = _maxLife;
                Debug.Log($"RemainingAircraft Count Decreaced {_remainingAircraft}");
            }
        }
    }

    public void Heal(int value)
    {
        _life += value;
        if (_life > _maxLife) { _life = _maxLife; }
    }
}
