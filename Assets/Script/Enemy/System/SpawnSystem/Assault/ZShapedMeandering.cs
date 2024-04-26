using UnityEngine;

/// <summary> Z字蛇行 </summary>
public class ZShapedMeandering : IEnemyGeneration
{
    private readonly float _straightMoveDistance = 10f;
    private readonly Vector2 _diagonalMoveDistance = Vector2.zero;

    public ZShapedMeandering(EnemyManager enemyManager)
    {
        _straightMoveDistance = enemyManager.MovementParam.StraightMoveDistance;
        _diagonalMoveDistance = enemyManager.MovementParam.DiagonalMoveDistance;
    }

    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;
        if (ZShaped(assault))
        {
            if (assault.ZShaped == ZShapedMove.Straight && assault.IsMoveUp)
            {
                assault.MoveDirection = new Vector2(1, 1);
                assault.ZShaped = ZShapedMove.Diagonal;
                assault.TargetPos = assault.Transform.position + new Vector3(_diagonalMoveDistance.x, _diagonalMoveDistance.y, 0f);
            }
            else if (assault.ZShaped == ZShapedMove.Straight && !assault.IsMoveUp)
            {
                assault.MoveDirection = new Vector2(1, -1);
                assault.ZShaped = ZShapedMove.Diagonal;
                assault.TargetPos = assault.Transform.position + new Vector3(_diagonalMoveDistance.x, -_diagonalMoveDistance.y, 0f);
            }
            else
            {
                assault.MoveDirection = Vector2.left;
                assault.ZShaped = ZShapedMove.Straight;
                assault.TargetPos = assault.Transform.position - new Vector3(_straightMoveDistance, 0f, 0f);
            }
        }
        assault.Rb2d.velocity = assault.MoveDirection * enemy.MoveSpeed;
    }

    private bool ZShaped(Assault assault)
    {
        if (assault.MoveDirection == Vector2.zero)
        {
            assault.MoveDirection = Vector2.left;
            assault.TargetPos = assault.Transform.position - new Vector3(_straightMoveDistance, 0f, 0f);
        }

        if (assault.ZShaped == ZShapedMove.Straight) { return assault.Transform.position.x <= assault.TargetPos.x; }
        else { return assault.IsMoveUp ?
                assault.Transform.position.y >= assault.TargetPos.y : assault.Transform.position.y <= assault.TargetPos.y; }
    }
}

public enum ZShapedMove
{
    Straight,
    Diagonal
}
