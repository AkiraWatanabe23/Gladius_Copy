using UnityEngine;

public class Jumping : IEnemyGeneration
{
    private readonly float _moveRadius = 1f;
    private readonly int _semicircleAttackCount = 1;

    public Jumping(EnemyManager enemyManager)
    {
        _moveRadius = enemyManager.MovementParam.JumpingHeight;
        _semicircleAttackCount = enemyManager.MovementParam.SemicircleAttackCount;
    }

    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Shot) { return; }

        var shot = (Shot)enemy.EnemySystem;
        //以下の式だと、1周あたり1秒かかる（経過時間 * 単位円の直径）
        //shot.Angle += Time.deltaTime * (2 * Mathf.PI);

        //移動速度を掛けることで、1周あたりにかける時間を短くしている（「1 / moveSpeed」秒になる）
        shot.Angle += Time.deltaTime * (2 * Mathf.PI) * enemy.MoveSpeed;

        //Cos(0) = 1 により座標がずれるのを -1 で防ぐ
        shot.Transform.position =
            shot.MoveInitPos + new Vector3(Mathf.Cos(shot.Angle) - _moveRadius, Mathf.Sin(shot.Angle), shot.Transform.position.z);
        if (MovementForSemicircle(shot.Angle))
        {
            shot.Angle = 0f;
            shot.MoveInitPos = shot.Transform.position;
            Attack(shot);
        }
    }

    /// <summary> 半周の動きをしたかどうかの判定を行う </summary>
    private bool MovementForSemicircle(float currentAngle) => currentAngle >= Mathf.PI;

    /// <summary> 半円を描くように弾を撃ちだす </summary>
    private void Attack(Shot shot)
    {
        //半円を弾数分だけ分割したときの1つあたりの角度
        var splitAngle = 180f / (_semicircleAttackCount - 1);
        Debug.Log(splitAngle);
        //半円を等分に割って弾を撃ちだす
        for (int i = 0; i < _semicircleAttackCount; i++)
        {
            var currentAngle = i * splitAngle;
            if (i == 0) { currentAngle = 0f; }
            else if (i == _semicircleAttackCount - 1) { currentAngle = 180f; }

            float angle = Mathf.Deg2Rad * (currentAngle);
            var direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);

            var spawnBullet = GameManager.Instance.BulletHolder.BulletsDictionary[InitialBulletType.Default];
            var bullet =
                GameManager.Instance.ObjectPool.SpawnObject(spawnBullet);
            bullet.transform.position = shot.Transform.position + direction;
            if (bullet.TryGetComponent(out BulletController bulletData))
            {
                bulletData.Initialize(
                    shot.Controller.MoveSpeed, shot.Controller.AttackValue, shot.Enemy.layer, direction);
            }
        }
    }
}
