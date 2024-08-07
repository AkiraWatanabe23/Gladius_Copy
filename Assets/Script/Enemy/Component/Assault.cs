﻿using System.Collections.Generic;
using UnityEngine;

public class Assault : IEnemy
{
    public EnemyController Controller { get; set; }
    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }
    public Rigidbody2D Rb2d { get; set; }
    public Vector2 MoveDirection { get; set; }

    #region Movement Unique Data
    public bool IsFindPlayer { get; set; }
    public bool IsFinishMoveUp { get; set; }
    public float Angle { get; set; }
    public Vector3 TargetPos { get; set; }
    public ZShapedMove ZShaped { get; set; } = ZShapedMove.Straight;
    public bool IsMoveUp { get; private set; }
    public bool IsChase { get; set; }
    public bool IsRotate { get; set; }
    public float InitialRotateValue { get; set; }
    public float RotateAngle { get; set; }
    public int CurrentRouteIndex { get; set; } = -1;
    public List<Vector3> MoveRoute { get; set; }

    public float InitialYPosition { get; set; }
    #endregion

    public void Init()
    {
        if (Enemy.TryGetComponent(out Rigidbody2D rb2d)) { Rb2d = rb2d; }
        else { Rb2d = Enemy.AddComponent<Rigidbody2D>(); }

        Rb2d.gravityScale = 0f;
        FlagParamReset();

        if (Controller.MovementType == EnemyMovementType.ZShapedMeandering) { IsMoveUp = Transform.position.y <= 0f; }
        else if (Controller.MovementType == EnemyMovementType.FollowTerrain)
        {
            Controller.PathDrawer.Initialize();
        }

        InitialYPosition = Transform.position.y;
    }

    private void FlagParamReset()
    {
        IsFindPlayer = false;
        IsFinishMoveUp = false;
        IsMoveUp = false;
        IsChase = false;
        IsRotate = false;
    }
}
