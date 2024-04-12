using Constants;
using UnityEngine;

/// <summary> 地形に沿う </summary>
public class FollowTerrain : IEnemyGeneration
{
    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;
        if (!assault.IsChase && (GameManager.Instance.PlayerTransform.position - assault.Transform.position).sqrMagnitude <= 1f)
        {
            assault.IsChase = true;
            assault.MoveDirection = (GameManager.Instance.PlayerTransform.position - assault.Transform.position).normalized;
        }
        else
        {
            if (assault.MoveRoute == null || assault.MoveRoute.Count <= 0) { Consts.Log("No route"); return; }

            if (assault.IsRotate) { Rotate(assault); }
            if (DirectionChange(assault))
            {
                assault.IsRotate = true;
                assault.InitialRotateValue = assault.Transform.localEulerAngles.z;
                assault.RotateAngle = GetRotateAngle(assault);

                RouteSetting(assault);
                assault.CurrentRouteIndex++;
            }
        }

        assault.Rb2d.velocity = assault.MoveDirection * enemy.MoveSpeed;
    }

    /// <summary> 次の移動方向を設定する </summary>
    private void RouteSetting(Assault assault)
    {
        if (assault.CurrentRouteIndex >= assault.MoveRoute.Count - 1) { return; }

        var direction
            = (assault.MoveRoute[assault.CurrentRouteIndex + 2] - assault.MoveRoute[assault.CurrentRouteIndex + 1]).normalized;
        if (assault.MoveDirection != (Vector2)direction)
        {
            assault.MoveDirection = direction;
        }
    }

    /// <summary> 移動方向の変更があるか調べる </summary>
    private bool DirectionChange(Assault assault)
    {
        var sqrtDistance
            = (assault.MoveRoute[assault.CurrentRouteIndex + 1] - assault.Transform.position).sqrMagnitude;

        return sqrtDistance <= 0.1f;
    }

    private void Rotate(Assault assault)
    {
        assault.Transform.Rotate(0f, 0f, Time.deltaTime * 90f);
        if (assault.Transform.localEulerAngles.z >= assault.InitialRotateValue + assault.RotateAngle)
        {
            assault.IsRotate = false;
        }
    }

    private float GetRotateAngle(Assault assault)
    {
        Vector2 current = assault.MoveDirection;
        Vector2 next
            = (assault.MoveRoute[assault.CurrentRouteIndex + 2] - assault.MoveRoute[assault.CurrentRouteIndex + 1]).normalized;

        // 2つのベクトルの成す角度をラジアンで取得
        float angleRad = Vector2.Angle(current, next);

        // ラジアンを度数に変換
        float angleDegrees = angleRad * Mathf.Rad2Deg;

        return angleDegrees;
    }
}
