/// <summary> ゲームオーバーの条件 </summary>
public class GameOver
{
    private int _life = 0;

    public void Init()
    {
        var playerHealth = GameManager.Instance.Player.Health;
        if (playerHealth.HealthType == HealthType.HP)
        {
            _life = playerHealth.HP.Life;
        }
        else if (playerHealth.HealthType == HealthType.RemainingAircraft)
        {
            _life = playerHealth.Aircraft.RemainingAircraftCount;
        }
    }

    /// <summary> ゲームオーバーの条件 -> 体力 or 残機が0 </summary>
    public bool GameOverCondition() => _life <= 0;
}
