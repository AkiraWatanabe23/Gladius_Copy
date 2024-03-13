using System.Collections.Generic;

public interface IEnemyGeneration
{
    public List<EnemyController> Enemies { get; set; }

    public void OnUpdate()
    {
        if (Enemies == null || Enemies.Count == 0) { return; }

        for (int i = Enemies.Count; i >= 0; i--) { Movement(Enemies[i]); }
    }

    public void Movement(EnemyController enemy);
}
