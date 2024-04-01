using UnityEngine;

public interface IEnemy
{
    public EnemyController Controller { get; set; }
    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }

    public void Init();
}
