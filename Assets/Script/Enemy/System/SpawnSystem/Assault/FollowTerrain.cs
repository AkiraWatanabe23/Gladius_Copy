using System;
using System.Collections.Generic;

/// <summary> 地形に沿う </summary>
[Serializable]
public class FollowTerrain : IEnemyGeneration
{
    public List<EnemyController> Enemies { get; set; }

    public void Movement(EnemyController enemy)
    {

    }
}
