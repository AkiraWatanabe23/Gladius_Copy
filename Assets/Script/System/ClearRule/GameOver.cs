/// <summary> ゲームオーバーの条件 </summary>
public class GameOver
{
    private PlayerHealth _healthInstance = default;

    public void Init()
    {
        _healthInstance = GameManager.Instance.Player.Health;
    }

    /// <summary> ゲームオーバーの条件 → 残機が0 </summary>
    public bool GameOverCondition() => _healthInstance.RemainingCount <= 0;
}
