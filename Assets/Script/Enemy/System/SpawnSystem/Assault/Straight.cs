using UnityEngine;

/// <summary> まっすぐ進行 </summary>
public class Straight : IEnemyGeneration
{
    protected EnemyManager EnemyManager { get; private set; }

    public Straight(EnemyManager enemyManager)
    {
        EnemyManager = enemyManager;
    }

    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;
        SearchPlayer(assault);

        if (!assault.IsFindPlayer) { assault.Rb2d.velocity = Vector2.left * enemy.MoveSpeed; }
        else { assault.Rb2d.velocity = assault.MoveDirection * enemy.MoveSpeed; }
    }

    private void SearchPlayer(Assault assault)
    {
        if (assault.IsFindPlayer) { return; }

        var player = EnemyManager.PlayerTransform;
        if (player.position.x < assault.Transform.position.x)
        {
            assault.IsFindPlayer = true;
            var direction = (player.position - assault.Transform.position).normalized;
            assault.MoveDirection = direction;
        }
    }
}
