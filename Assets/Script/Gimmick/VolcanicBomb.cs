using UnityEngine;

public class VolcanicBomb : MonoBehaviour
{
    private int _attackValue = 1;

    public void Initialize(int attackValue) { _attackValue = attackValue; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.PlaySE(SEType.EruptionFalled);
        if (collision.gameObject.TryGetComponent(out IDamageable target))
        {
            target.ReceiveDamage(_attackValue);
            GameManager.Instance.ObjectPool.RemoveObject(gameObject);
        }
        else if (collision.gameObject.TryGetComponent(out Ground _) ||
                 collision.gameObject.TryGetComponent(out BulletController _))
        {
            GameManager.Instance.ObjectPool.RemoveObject(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        GameManager.Instance.ObjectPool.RemoveObject(gameObject);
    }
}
