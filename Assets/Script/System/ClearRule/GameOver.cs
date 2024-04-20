/// <summary> ゲームオーバーの条件 </summary>
public class GameOver
{
    private int _life = 0;

    public void Init()
    {
        var playerHealth = GameManager.Instance.Player.Health;
        if (playerHealth.HealthType == HealthType.HP)
        {
            _life = playerHealth.Life;
        }
        else if (playerHealth.HealthType == HealthType.RemainingAircraft)
        {
            _life = playerHealth.RemainingAircraft;
        }
    }

    public bool GameOverCondition() => _life <= 0;
}
