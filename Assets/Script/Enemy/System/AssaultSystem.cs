using System.Collections.Generic;
using UnityEngine;

public class AssaultSystem : EnemySystemBase
{
    private List<Assault> _assaultEnemies = default;

    public override void Initialize()
    {
        if (EnemyCommon.AssaultEnemies == null) { return; }
        _assaultEnemies = EnemyCommon.AssaultEnemies;

        if (_assaultEnemies == null || _assaultEnemies.Count <= 0) { return; }
        foreach (var target in _assaultEnemies)
        {
            if (target.Enemy.TryGetComponent(out Rigidbody2D rb2d)) { target.Rb2d = rb2d; }
            else { target.Rb2d = target.Enemy.AddComponent<Rigidbody2D>(); }

            target.Rb2d.gravityScale = 0f;
        }
    }

    public override void OnUpdate()
    {
        if (_assaultEnemies == null || _assaultEnemies.Count == 0) { return; }
        for (int i = _assaultEnemies.Count - 1; i >= 0; i--)
        {
            if (_assaultEnemies[i] == null) { continue; }
            Movement(_assaultEnemies[i]);
        }
    }

    public override void AddEnemy(IEnemy target)
    {
        if (target is not Assault) { return; }

        _assaultEnemies.Add((Assault)target);
    }

    public override void RemoveEnemy(IEnemy target)
    {
        if (target is not Assault) { return; }

        _assaultEnemies.Remove((Assault)target);
    }

    private void Movement(Assault enemy)
    {
        switch (enemy.EnemyController.MovementType)
        {
            case EnemyMovementType.Straight: break;
            case EnemyMovementType.RightAngle: break;
            case EnemyMovementType.FigureEight: break;
            case EnemyMovementType.ZShapedMeandering: break;
            case EnemyMovementType.FollowTerrain: break;
        }
    }
}
