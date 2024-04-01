using UnityEngine;

/// <summary> 直角に曲がる </summary>
public class RightAngle : IEnemyGeneration
{
    protected EnemyManager EnemyManager { get; private set; }

    public RightAngle(EnemyManager enemyManager)
    {
        EnemyManager = enemyManager;
    }

    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;
        if (!assault.IsFinishMoveUp)
        {
            assault.Rb2d.velocity = Vector2.up * enemy.MoveSpeed;
            assault.IsFinishMoveUp = MovementSearch(assault);
        }
        else
        {
            assault.Rb2d.velocity = Vector2.left * enemy.MoveSpeed;
        }
    }

    private bool MovementSearch(Assault assault)
        => Mathf.Abs(assault.Transform.position.y - EnemyManager.PlayerTransform.position.y) < 0.1f;
}
