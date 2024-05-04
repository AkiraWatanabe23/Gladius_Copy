using System.Collections;
using UnityEngine;

public class CrawlGround : IEnemyGeneration
{
    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Shot) { return; }

        var shot = (Shot)enemy.EnemySystem;
        if (shot.IsEnterArea && !shot.IsAiming)
        {
            shot.IsAiming = true;
            shot.Measuring = Aiming(shot);
        }
        if (shot.Measuring != null && !shot.Measuring.MoveNext()) { shot.Measuring = null; }

        if (!shot.IsAiming) { shot.Rb2d.velocity = shot.MoveDirection * enemy.MoveSpeed; }
    }

    private IEnumerator Aiming(Shot shot)
    {
        var muzzleDirection = (shot.PlayerTransform.position - shot.Transform.position).normalized;
        shot.Transform.forward = muzzleDirection;
        shot.AimingTimer += Time.deltaTime;

        yield return new WaitUntil(() => shot.AimingTimer >= shot.AttackInterval);
        shot.AimingTimer = 0f;
        Attack(shot);
    }

    private void Attack(Shot shot)
    {
        var bulletPrefab
            = GameManager.Instance.ObjectPool.SpawnObject(GameManager.Instance.BulletHolder.EnemyBullet);

        var spawnPos = shot.ShotMuzzle == null ? shot.Transform.position : shot.ShotMuzzle.position;
        bulletPrefab.transform.position = spawnPos;

        var bulletData = bulletPrefab.GetComponent<BulletController>();
        bulletData.Initialize(shot.Controller.AttackValue, shot.Enemy.layer);
    }
}
