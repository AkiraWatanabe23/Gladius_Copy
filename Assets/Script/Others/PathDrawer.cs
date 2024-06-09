using Constants;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathDrawer
{
    [SerializeField]
    private Transform _startPoint = default;
    [SerializeField]
    private Transform _goalPoint = default;
    [Tooltip("通過地点")]
    [SerializeField]
    private Transform[] _intermediatePoints = default;

    /// <summary> 通過地点を格納したList </summary>
    private List<Vector3> _pathPoints = default;

    public List<Vector3> PathPoints => _pathPoints;

    public void Initialize() => DrawPath();

    private void DrawPath()
    {
        if (_startPoint == null || _goalPoint == null)
        {
            Consts.LogWarning("Start point or end point not assigned.");
            return;
        }

        //経路を格納
        _pathPoints = new List<Vector3>()
        {
            _startPoint.position
        };
        foreach (Transform point in _intermediatePoints)
        {
            _pathPoints.Add(point.position);
        }
        _pathPoints.Add(_goalPoint.position);
    }
}
