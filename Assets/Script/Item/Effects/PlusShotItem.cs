using Constants;
using UnityEngine;

public class PlusShotItem : IGameItem
{
    [SerializeField]
    private PlusShotType _plusShotType = PlusShotType.Missile;
    [Tooltip("プラスショットが永続の効果か")]
    [SerializeField]
    private bool _isEffectPermanent = false;
    [Tooltip("プラスショットの持続時間（_isEffectPermanent == false の場合）")]
    [SerializeField]
    private float _effectDuration = 1f;

    public void Initialize() { }

    public void PlayEffect()
    {
        AudioManager.Instance.PlaySE(SEType.AddPlusShot);
        //ショット追加
        GameManager.Instance.Player.Attack.PlusShotBullet = _plusShotType;
        if (_plusShotType == PlusShotType.SupportShot) { GenerateSupport(); }
    }

    private void GenerateSupport()
    {
        if (GameManager.Instance.CurrentSupportCount >= GameManager.Instance.MaxSupportCount)
        {
            Consts.Log("これ以上補助兵装を増やせません");
            return;
        }
        GameManager.Instance.CurrentSupportCount++;

        var support = GameManager.Instance.ObjectPool.SpawnObject(
            GameManager.Instance.BulletHolder.PlusShotsDictionary[PlusShotType.SupportShot]);

        support.transform.position = GameManager.Instance.PlayerTransform.position;

        var bulletData = support.GetComponent<BulletController>();
        bulletData.Initialize(1f, 0, GameManager.Instance.Player.Attack.Layer);
    }
}
