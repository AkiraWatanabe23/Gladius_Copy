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
        //ショット追加
    }
}
