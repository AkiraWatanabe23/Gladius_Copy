using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CrawlGround : IEnemyGeneration
{
    private Transform _player = GameManager.Instance.Player.transform;
    private const float _minDistance = 0.1f; // プレイヤーとの最小距離
    private const float _smoothTime = 0.3f; // スムーズな移動のための時間

    private float _velocityX = 0f;

    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Shot) { return; }

        var shot = (Shot)enemy.EnemySystem;

        if (!shot.IsAiming)
        {
            // プレイヤーに近づくが、ある程度の距離を保つ
            float distanceX = Mathf.Abs(_player.position.x - enemy.transform.position.x);
            if (distanceX > _minDistance)
            {
                // スムーズな移動
                float targetPositionX = Mathf.SmoothDamp(enemy.transform.position.x, _player.position.x, ref _velocityX, _smoothTime);
                Vector2 targetPosition = new Vector2(targetPositionX, enemy.transform.position.y);
                shot.Rb2d.velocity = (targetPosition - (Vector2)enemy.transform.position).normalized * enemy.MoveSpeed;
            }
            else
            {
                // 停止するか、別の行動をさせる場合
                shot.Rb2d.velocity = Vector2.zero;
            }
        }

        if (shot.IsEnterArea && !shot.IsAiming)
        {
            shot.IsAiming = true;
            shot.Measuring = Aiming(shot);
            enemy.StartCoroutine(shot.Measuring); // Aimingメソッドのコルーチンを開始
            shot.Rb2d.velocity = Vector2.zero;
        }
    }

    private IEnumerator Aiming(Shot shot)
    {
        var muzzleDirection = (shot.PlayerTransform.position - shot.Transform.position).normalized;
        shot.ShotMuzzle.position = shot.Transform.position;
        shot.ShotMuzzle.forward = muzzleDirection;

        // インターバルを設ける
        yield return new WaitForSeconds(shot.AttackInterval);

        // 一発撃つ
        Attack(shot);

        // インターバルを設ける
        yield return new WaitForSeconds(shot.AttackInterval);

        // 再び移動できるようにする
        shot.IsAiming = false;
    }

    private void Attack(Shot shot)
    {
        var bulletPrefab = GameManager.Instance.ObjectPool.SpawnObject(GameManager.Instance.BulletHolder.EnemyBullet);

        var spawnTransform = shot.ShotMuzzle == null ? shot.Transform : shot.ShotMuzzle;
        bulletPrefab.transform.position = spawnTransform.position;

        var bulletData = bulletPrefab.GetComponent<BulletController>();
        bulletData.Initialize(shot.Controller.AttackValue, shot.Enemy.layer, spawnTransform.forward);
    }
}
