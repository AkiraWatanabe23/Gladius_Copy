using Constants;
using System.Collections;
using UnityEngine;

/// <summary> ワープホール </summary>
public class WarpHoleGimmick : MonoBehaviour
{
    [Tooltip("自機衝突時に与えるダメージ")]
    [SerializeField]
    private int _damageValue = 1;
    [Tooltip("一番上に設定した場所に出現する")]
    [SerializeField]
    private Transform[] _warpExit = new Transform[3];

    private void Start() => AudioManager.Instance.PlaySE(SEType.WarpInitialized);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable target))
        {
            target.ReceiveDamage(_damageValue);
        }
        else if (collision.gameObject.TryGetComponent(out BulletController _))
        {
            StartCoroutine(Warp(collision.gameObject.transform));
        }
    }

    /// <summary> ワープ処理（位置の強制移動） </summary>
    private IEnumerator Warp(Transform target)
    {
        Transform exit = null;
        if (_warpExit == null || _warpExit.Length <= 0) { Consts.LogWarning("移動先の指定がありません"); yield break; }
        else
        {
            for (int i = 0; i < _warpExit.Length; i++)
            {
                if (_warpExit[i] != null) { exit = _warpExit[i]; break; }
            }
        }

        AudioManager.Instance.PlaySE(SEType.WarpEnterBullet);
        target.gameObject.SetActive(false);

        //ここで入口のワープが消えるようなAnimation
        yield return null;
        target.transform.position = exit.position;
        target.gameObject.SetActive(true);
        AudioManager.Instance.PlaySE(SEType.WarpExitBullet);
        //出口のワープが消えるAnimation
        yield return null;
    }

    private void OnDestroy() => AudioManager.Instance.PlaySE(SEType.WarpDestroyed);
}
