using UnityEngine;

// 日本語対応
public class DefautBullet : BulletBase
{
    private void OnEnable()
    {
        OnTriggerEnterEvent += Attack;
    }

    private void Update()
    {
        Rigidbody2D.velocity = Vector2.right * Speed;
    }

    private void OnDisable()
    {
        OnTriggerEnterEvent -= Attack;
    }

    private void Attack(GameObject target)
    {
        if (target == null) return;
        if (!target.TryGetComponent(out IDamageable damageTarget)) return;

        damageTarget.ReceiveDamage(Damage);
    }
}
