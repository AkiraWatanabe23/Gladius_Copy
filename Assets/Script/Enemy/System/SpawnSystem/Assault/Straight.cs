using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> まっすぐ進行 </summary>
[Serializable]
public class Straight : IEnemyGeneration
{
    public List<EnemyController> Enemies { get; set; }

    public void Movement(EnemyController enemy)
    {
        if(enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;

        if (!assault.IsFindPlayer) { assault.Rb2d.velocity = Vector2.left * assault.MoveSpeed; }
        else
        {

        }
    }
}
