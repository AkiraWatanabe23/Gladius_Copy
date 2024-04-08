using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    [SerializeField]
    private Transform _startPoint = default;
    [SerializeField]
    private Transform _goalPoint = default;
    [Tooltip("通過地点")]
    [SerializeField]
    private List<Transform> _intermediatePoints = default;
    [SerializeField]
    private Transform _moveObj = default;
    [SerializeField]
    private LineRenderer _lineRenderer = default;
    [SerializeField]
    private float _moveSpeed = 5f;

    /// <summary> 通過地点を格納したList </summary>
    private List<Vector3> _pathPoints = default;

    private void Start()
    {
        DrawPath();
        MoveObjects();
    }

    private void DrawPath()
    {
        if (_startPoint == null || _goalPoint == null || _lineRenderer == null)
        {
            Debug.LogError("Start point, end point, or line renderer not assigned.");
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

        //描画設定
        _lineRenderer.positionCount = _pathPoints.Count;
        _lineRenderer.SetPositions(_pathPoints.ToArray());
    }

    private void MoveObjects()
    {
        if (_pathPoints == null || _pathPoints.Count == 0 || _moveObj == null)
        {
            Debug.LogError("Path points or objects to move not assigned.");
            return;
        }
        _moveObj.position = _startPoint.position;
        StartCoroutine(MoveObjectAlongPath(_moveObj));
    }

    private IEnumerator MoveObjectAlongPath(Transform obj)
    {
        for (int i = 0; i < _pathPoints.Count - 1; i++)
        {
            float distanceAlongSegment = 0f;
            float totalDistance = Vector3.Distance(_pathPoints[i], _pathPoints[i + 1]);
            while (distanceAlongSegment < totalDistance)
            {
                float step = _moveSpeed * Time.deltaTime;
                obj.position = Vector3.MoveTowards(obj.position, _pathPoints[i + 1], step);
                distanceAlongSegment += step;
                yield return null;
            }
        }
    }
}
