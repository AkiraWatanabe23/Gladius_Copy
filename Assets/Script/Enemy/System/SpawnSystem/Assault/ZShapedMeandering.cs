using System;
using UnityEngine;

/// <summary> Z字蛇行 </summary>
[Serializable]
public class ZShapedMeandering : IEnemyGeneration
{
    [Tooltip("横移動時の移動時間")]
    [SerializeField]
    private float _straightMoveSec = 2f;
    [Tooltip("斜め移動時の移動時間")]
    [SerializeField]
    private float _diagonalMoveSec = 1f;

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
