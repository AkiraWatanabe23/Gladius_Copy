using UnityEngine;

// 日本語対応
public class DefautBullet : BulletBase
{
    private void OnEnable()
    {
        
    }

    private void Update()
    {
        Rigidbody2D.velocity = Vector2.left * Speed;
    }

    private void OnDisable()
    {
        
    }

    private void Attack(GameObject target)
    {
        if (target == null) return;
        if (!target.TryGetComponent(out IDamageable damageTarget)) return;

        damageTarget.ReceiveDamage(Damage);
    }
}
