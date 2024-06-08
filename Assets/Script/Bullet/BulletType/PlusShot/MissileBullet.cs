using UnityEngine;

/// <summary> ミサイル弾 </summary>
public class MissileBullet : IBulletData
{
    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public int GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        Rb2d.velocity = MoveDirection * Speed;
    }

    public void Hit(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground _))
        {
            ChangeDirection(collision.gameObject);
        }
        else
        {
            if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

            damageTarget.ReceiveDamage(AttackValue);
            GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
        }
    }

    /// <summary> 地面等に衝突したときの方向転換処理 </summary>
    private void ChangeDirection(GameObject hitTarget)
    {
        //弾の回転を更新
        var bulletRot = Transform.localEulerAngles;
        var targetRot = hitTarget.transform.localEulerAngles;

        if (Mathf.Approximately(targetRot.z, 0f)) { bulletRot.z = 90f; }
        else { bulletRot.z += targetRot.z; }

        Transform.localEulerAngles = bulletRot;

        //弾の移動方向を更新
        MoveDirection = hitTarget.transform.right;
    }
}
