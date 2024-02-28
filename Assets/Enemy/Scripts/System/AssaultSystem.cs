using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssaultSystem : EnemySystemBase
{
    private readonly List<Assault> _assaultEnemies = default;

    public AssaultSystem(params Assault[] targets)
    {
        _assaultEnemies = new();
        _assaultEnemies = targets.ToList();
        foreach (var target in targets)
        {
            if (target.Enemy.TryGetComponent(out Rigidbody2D rb2d)) { target.Rb2d = rb2d; }
            else { target.Rb2d = target.Enemy.AddComponent<Rigidbody2D>(); }

            target.Rb2d.gravityScale = 0f;
        }
    }

    public override void OnUpdate()
    {
        for (int i = _assaultEnemies.Count - 1; i >= 0; i--)
        {
            if (_assaultEnemies[i] == null) { continue; }
            _assaultEnemies[i].Rb2d.velocity = Vector2.left * _assaultEnemies[i].MoveSpeed;
        }
    }

    public void AddEnemy(params Assault[] targets)
    {
        foreach (var enemy in targets) { _assaultEnemies.Add(enemy); }
    }
}
