using System.Collections.Generic;
using UnityEngine;

public class AssaultSystem : EnemySystemBase
{
    private List<Assault> _assaultEnemies = default;

    private Straight _straightMovement = default;
    private RightAngle _rightAngle = default;
    private FigureEight _figureEight = default;
    private ZShapedMeandering _zShapedMeandering = default;
    private FollowTerrain _followTerrain = default;

    public override void Initialize(EnemyManager enemyManager)
    {
        EnemyManager = enemyManager;

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
        _assaultEnemies ??= new();
        _assaultEnemies.Add((Assault)target);

        target.Init();
    }

    public override void RemoveEnemy(IEnemy target) { _assaultEnemies.Remove((Assault)target); }

    private void Movement(Assault enemy)
    {
        switch (enemy.Controller.MovementType)
        {
            case EnemyMovementType.Straight:
                _straightMovement ??= new(EnemyManager); _straightMovement.Movement(enemy.Controller); break; 
            case EnemyMovementType.RightAngle:
                _rightAngle ??= new(EnemyManager); _rightAngle.Movement(enemy.Controller); break;
            case EnemyMovementType.FigureEight:
                _figureEight ??= new(); _figureEight.Movement(enemy.Controller); break;
            case EnemyMovementType.ZShapedMeandering: break;
            case EnemyMovementType.FollowTerrain: break;
        }
    }
}
