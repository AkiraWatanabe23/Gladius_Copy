using UnityEngine;

public class Assault : IEnemy
{
    public Rigidbody2D Rb2d { get; set; }
    public EnemyController EnemyController { get; set; }
    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }
    public bool IsFindPlayer { get; set; }

    public void Init()
    {
        if (Enemy.TryGetComponent(out Rigidbody2D rb2d)) { Rb2d = rb2d; }
        else { Rb2d = Enemy.AddComponent<Rigidbody2D>(); }

        Rb2d.gravityScale = 0f;
    }
}
