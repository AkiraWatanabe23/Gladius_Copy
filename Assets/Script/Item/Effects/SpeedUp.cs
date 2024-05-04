using UnityEngine;

/// <summary> 移動速度上昇 </summary>
public class SpeedUp : IGameItem
{
    [Range(1f, 5f)]
    [Tooltip("移動速度の上昇倍率")]
    [SerializeField]
    private float _speedUpValue = 1f;

    private PlayerController _player = default;

    public void Initialize()
    {
        _player = GameManager.Instance.Player;
    }

    public void PlayEffect()
    {
        AudioManager.Instance.PlaySE(SEType.SpeedUp);
        _player.Movement.SpeedUp(_speedUpValue);
    }
}
