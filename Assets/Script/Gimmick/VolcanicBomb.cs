using UnityEngine;

public class VolcanicBomb : MonoBehaviour
{
    private Animator _anim;
    private int _attackValue = 1;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        StartCoroutine(WaitForAnimationToEnd());
    }

    public void Initialize(int attackValue) { _attackValue = attackValue; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.PlaySE(SEType.EruptionFalled);
        if (collision.gameObject.TryGetComponent(out IDamageable target))
        {
            target.ReceiveDamage(_attackValue);
        }/*
        else if (collision.gameObject.TryGetComponent(out Ground _) ||
                 collision.gameObject.TryGetComponent(out BulletController _))
        {
            GameManager.Instance.ObjectPool.RemoveObject(gameObject);
        }*/
    }

    private System.Collections.IEnumerator WaitForAnimationToEnd()
    {
        // アニメーションが再生中であれば待機する
        while (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f ||
               _anim.IsInTransition(0))
        {
            yield return null;
        }

        // アニメーションが終了した後の処理を実行する
        GameManager.Instance.ObjectPool.RemoveObject(gameObject);
    }

    private void OnBecameInvisible()
    {
        GameManager.Instance.ObjectPool.RemoveObject(gameObject);
    }
}
