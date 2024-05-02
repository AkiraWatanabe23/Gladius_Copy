using UnityEngine;

/// <summary> 体力回復 </summary>
public class HealLife : IGameItem
{
    [Tooltip("回復する値")]
    [SerializeField]
    private int _healValue = 1;

    private PlayerController _player = default;

    public void Initialize()
    {
        _player = GameManager.Instance.Player;
    }

    public void PlayEffect()
    {
        AudioManager.Instance.PlaySE(SEType.Heal);
        _player.Health.Heal(_healValue);
    }
}
