using System;
using System.Collections.Generic;

/// <summary> 直角に曲がる </summary>
[Serializable]
public class RightAngle : IEnemyGeneration
{
    public List<EnemyController> Enemies { get; set; }

    public void Movement(EnemyController enemy)
    {

    }
}
