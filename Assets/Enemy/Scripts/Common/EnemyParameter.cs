using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/CreateEnemyParamAsset")]
public class EnemyParameter : ScriptableObject
{
    [field: SerializeField]
    public EnemyType EnemyType { get; private set; } = EnemyType.None;
    [field: SerializeField]
    public int AttackValue { get; private set; }
}
