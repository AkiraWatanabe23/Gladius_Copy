using Constants;
using UnityEngine;

public class PlusShotItem : IGameItem
{
    [SerializeField]
    private PlusShotType _plusShotType = PlusShotType.Missile;
    [Tooltip("プラスショットの効果持続時間")]
    [SerializeField]
    private float _effectDuration = 1f;

    public void Initialize() { }

    public void PlayEffect()
    {
        AudioManager.Instance.PlaySE(SEType.AddPlusShot);
        //ショット追加
        var player = GameManager.Instance.Player;

        player.Attack.PlusShotEffectTime = _effectDuration;
        player.Attack.PlusShotBullet = _plusShotType;
        if (_plusShotType == PlusShotType.SupportShot) { GenerateSupport(); }
        else if (_plusShotType == PlusShotType.Barrier) { GenerateBarrier(); }
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
        bulletData.Initialize(0, GameManager.Instance.Player.gameObject.layer, Vector2.zero);
    }

    private void GenerateBarrier()
    {
        var support = GameManager.Instance.ObjectPool.SpawnObject(
            GameManager.Instance.BulletHolder.PlusShotsDictionary[PlusShotType.Barrier]);

        support.transform.position = GameManager.Instance.Player.Attack.Muzzle.position;

        var bulletData = support.GetComponent<BulletController>();
        bulletData.Initialize(0, GameManager.Instance.Player.gameObject.layer, Vector2.zero);
    }
}
