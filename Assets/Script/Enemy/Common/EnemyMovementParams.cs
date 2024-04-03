using System;
using UnityEngine;

/// <summary> 各Enemyの動きの際に設定する値 </summary>
[Serializable]
public class EnemyMovementParams
{
    [Header("AssaultEnemy")]
    [Tooltip("横移動時の移動時間")]
    [SerializeField]
    private float _straightMoveSec = 2f;
    [Tooltip("斜め移動時の移動時間")]
    [SerializeField]
    private float _diagonalMoveSec = 1f;

    [Header("ShotEnemy")]
    [Tooltip("ジャンプする高さ")]
    [SerializeField]
    private float _jumpingHeight = 1f;
    [Tooltip("ジャンプ後に撃ちだす弾の数")]
    [Min(1)]
    [SerializeField]
    private int _semicircleAttackCount = 1;

    public float StraightMoveSec => _straightMoveSec;
    public float DiagonalMoveSec => _diagonalMoveSec;
    public float JumpingHeight => _jumpingHeight;
    public int SemicircleAttackCount => _semicircleAttackCount;
}
