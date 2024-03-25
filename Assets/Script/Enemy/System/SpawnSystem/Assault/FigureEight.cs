using System;
using System.Collections.Generic;

/// <summary> 8の字 </summary>
[Serializable]
public class FigureEight : IEnemyGeneration
{
    public List<EnemyController> Enemies { get; set; }

    public void Movement(EnemyController enemy)
    {

    }
}
