using UnityEngine;

/// <summary> 8の字 </summary>
public class FigureEightDown : IEnemyGeneration
{
    private float _amplitude = 3;

    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;

        //assault.Angle += Time.deltaTime;
        assault.Angle += Mathf.Clamp(Time.deltaTime, 0f, 0.02f);

        // Y座標を下上の順番に変更
        assault.Transform.position = new Vector3(
            assault.Transform.position.x,
            assault.InitialYPosition - Mathf.Sin(assault.Angle * (1 / enemy.MoveSpeed) * 2 * Mathf.PI) * 5, // 初期Y座標を基準に下上動
            assault.Transform.position.z
        );

        assault.Rb2d.velocity = Vector2.left * enemy.MoveSpeed;
    }
}
