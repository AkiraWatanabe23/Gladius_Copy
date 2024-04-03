using UnityEngine;

/// <summary> Z字蛇行 </summary>
public class ZShapedMeandering : IEnemyGeneration
{
    private readonly float _straightMoveSec = 2f;
    private readonly float _diagonalMoveSec = 1f;

    public ZShapedMeandering()
    {
        _straightMoveSec = GameManager.Instance.EnemyMovementParams.StraightMoveSec;
        _diagonalMoveSec = GameManager.Instance.EnemyMovementParams.DiagonalMoveSec;
    }

    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;
        if (ZShaped(assault))
        {
            if (assault.ZShaped == ZShapedMove.Straight)
            {
                assault.MoveDirection = new Vector2(1, -1);
                assault.ZShaped = ZShapedMove.Diagonal;
            }
            else
            {
                assault.MoveDirection = Vector2.left;
                assault.ZShaped = ZShapedMove.Straight;
            }
            assault.MovementTimer = 0f;
        }
        assault.Rb2d.velocity = assault.MoveDirection * enemy.MoveSpeed;
    }

    private bool ZShaped(Assault assault)
    {
        if (assault.MoveDirection == Vector2.zero) { assault.MoveDirection = Vector2.left; }

        assault.MovementTimer += Time.deltaTime;
        if (assault.ZShaped == ZShapedMove.Straight) { return assault.MovementTimer >= _straightMoveSec; }
        else { return assault.MovementTimer >= _diagonalMoveSec; }
    }
}

public enum ZShapedMove
{
    Straight,
    Diagonal
}
