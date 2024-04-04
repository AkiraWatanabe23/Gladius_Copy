using System;
using UnityEngine;

/// <summary> 各Enemyの動きの際に設定する値 </summary>
[Serializable]
public class EnemyMovementParams
{
    [Header("AssaultEnemy")]
    [Tooltip("横移動時の移動距離")]
    [SerializeField]
    private float _straightMoveDistance = 10f;
    [Tooltip("斜め移動時の移動距離（x : 横、y : 縦）")]
    [SerializeField]
    private Vector2 _diagonalMoveDistance = Vector2.zero;

    [Header("ShotEnemy")]
    [Tooltip("ジャンプする高さ")]
    [SerializeField]
    private float _jumpingHeight = 1f;
    [Tooltip("ジャンプ後に撃ちだす弾の数")]
    [Min(1)]
    [SerializeField]
    private int _semicircleAttackCount = 1;

    public float StraightMoveDistance => _straightMoveDistance;
    public Vector2 DiagonalMoveDistance => _diagonalMoveDistance;
    public float JumpingHeight => _jumpingHeight;
    public int SemicircleAttackCount => _semicircleAttackCount;
}
