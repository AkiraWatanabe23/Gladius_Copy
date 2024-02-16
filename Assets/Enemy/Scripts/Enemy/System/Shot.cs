using UnityEngine;

public class Shot : EnemySystemBase
{
    private readonly int _attackValue = 0;

    private bool _isEnterArea = false;

    public Shot(int attackValue)
    {
        _attackValue = attackValue;
    }

    public override void EnemyMovement()
    {
        if (!_isEnterArea) { return; }

        Debug.Log("Shot System");
    }
}
