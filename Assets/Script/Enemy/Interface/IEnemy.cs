using UnityEngine;

public interface IEnemy
{
    public EnemyController EnemyController { get; set; }
    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }

    public void Init();
}
