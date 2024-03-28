using UnityEngine;

/// <summary> シーン上のEnemyをすべて破壊 </summary>
public class Annihilation : IGameItem
{
    public void Initialize() { }

    public void PlayEffect()
    {
        var enemies = Object.FindObjectsOfType<EnemyController>();
        foreach (var enemy in enemies) { EnemyManager.Instance.ObjectPool.RemoveObject(enemy.gameObject); }
    }
}
