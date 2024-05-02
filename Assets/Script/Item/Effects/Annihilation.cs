using UnityEngine;

/// <summary> シーン上のEnemyをすべて破壊 </summary>
public class Annihilation : IGameItem
{
    public void Initialize() { }

    public void PlayEffect()
    {
        AudioManager.Instance.PlaySE(SEType.EnemyAnnihilation);
        var enemies = Object.FindObjectsOfType<EnemyController>();
        foreach (var enemy in enemies) { GameManager.Instance.ObjectPool.RemoveObject(enemy.gameObject); }
    }
}
