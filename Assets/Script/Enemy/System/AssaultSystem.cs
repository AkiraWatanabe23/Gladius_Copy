using System.Collections.Generic;
using UnityEngine;

public class AssaultSystem : EnemySystemBase
{
    private List<Assault> _assaultEnemies = default;

    private Straight _straightMovement = default;
    private RightAngle _rightAngle = default;
    private FigureEightUp _figureEightUp = default;
    private FigureEightDown _figureEightDown = default;
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
            if (_assaultEnemies[i] == null || !_assaultEnemies[i].Enemy.activeSelf) { continue; }
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
            case EnemyMovementType.FigureEightUp:
                _figureEightUp ??= new(); _figureEightUp.Movement(enemy.Controller); break;
            case EnemyMovementType.FigureEightDown:
                _figureEightDown ??= new(); _figureEightDown.Movement(enemy.Controller); break;
            case EnemyMovementType.ZShapedMeandering:
                _zShapedMeandering ??= new(EnemyManager); _zShapedMeandering.Movement(enemy.Controller); break;
            case EnemyMovementType.FollowTerrain:
                _followTerrain ??= new(); _followTerrain.Movement(enemy.Controller); break;
        }
    }
}
