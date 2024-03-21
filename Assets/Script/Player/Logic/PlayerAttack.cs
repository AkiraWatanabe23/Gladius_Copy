using System;
using UnityEngine;

[Serializable]
public class PlayerAttack : PlayerSystemBase
{
    [SerializeField]
    private int _attackValue = 1;
    [SerializeField]
    private float _attackInterval = 1f;
    [SerializeField]
    private BulletController _bulllet = default;

    public int AttackValue => _attackValue;

    protected bool IsGetShootInput => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

    public override void OnUpdate()
    {
        if (IsGetShootInput) { Attack(); }
    }

    private void Attack()
    {
        Debug.Log("attack");
    }
}
