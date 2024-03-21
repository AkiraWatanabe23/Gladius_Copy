using System;
using UnityEngine;

[Serializable]
public class PlayerHealth :  PlayerSystemBase
{
    [SerializeField]
    private int _life = 1;
    [SerializeField]
    private int _maxLife = 1;

    public override void Initialize(GameObject go)
    {
        _life = _maxLife;
    }

    public void ReceiveDamage(int value)
    {
        _life -= value;
        if (_life <= _maxLife)
        {
            //dead
        }
    }
}
