/// <summary> 地形に沿う </summary>
public class FollowTerrain : IEnemyGeneration
{
    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;
    }
}
