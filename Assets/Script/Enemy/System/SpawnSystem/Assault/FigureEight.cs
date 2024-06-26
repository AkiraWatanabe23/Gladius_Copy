﻿using UnityEngine;

/// <summary> 8の字 </summary>
public class FigureEight : IEnemyGeneration
{
    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;

        assault.Angle += Time.deltaTime;

        assault.Transform.position = new Vector3(
            assault.Transform.position.x,
            Mathf.Sin(assault.Angle * (1 / enemy.MoveSpeed) * 2 * Mathf.PI),
            assault.Transform.position.z
        );
        assault.Rb2d.velocity = Vector2.left * enemy.MoveSpeed;
    }
}
