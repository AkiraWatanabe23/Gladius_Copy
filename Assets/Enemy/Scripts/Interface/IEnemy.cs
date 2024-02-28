using UnityEngine;

public interface IEnemy
{
    public int HP { get; set; }
    public int AttackValue { get; }
    public float MoveSpeed { get; }

    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }
}
