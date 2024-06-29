/// <summary> ゲームオーバーの条件 </summary>
public class GameOver
{
    private PlayerHealth _healthInstance = default;

    public void Init()
    {
        _healthInstance = GameManager.Instance.Player.Health;
    }

    /// <summary> ゲームオーバーの条件 -> 体力 or 残機が0 </summary>
    public bool GameOverCondition()
    {
        if (_healthInstance.HealthType == HealthType.HP)
        {
           return _healthInstance.HP.Life <= 0;
        }
        else
        {
            return _healthInstance.Aircraft.RemainingAircraftCount <= 0;
        }
    }
}
