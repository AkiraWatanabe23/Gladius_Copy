using UnityEngine;

public class Fan : MonoBehaviour
{
    [Header("以下自機の赤色の枠に関するデータ")]
    [Header("扇の角度")]
    [Min(1f)]
    [SerializeField]
    private float _radius = 1f;
    [Header("扇の半径")]
    [Range(0f, 90f)]
    [SerializeField]
    private float _angle = 0f;

    private PolygonCollider2D _triangleCollider = default;

    public float Angle => _angle;

    private void Start()
    {
        _triangleCollider = GetComponent<PolygonCollider2D>();
        SetupTriangleCollider(_triangleCollider);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var radian = _angle * Mathf.Deg2Rad;
        var upperPos = transform.position + new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)) * _radius;
        var lowerPos = transform.position + new Vector3(Mathf.Cos(radian), -Mathf.Sin(radian)) * _radius;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, upperPos);
        Gizmos.DrawLine(transform.position, lowerPos);
        Gizmos.DrawLine(upperPos, lowerPos);
    }
#endif

    private void SetupTriangleCollider(PolygonCollider2D collider)
    {
        collider.isTrigger = true;
        var radian = _angle * Mathf.Deg2Rad;

        var vertices = collider.GetPath(0);
        vertices[0] = transform.localPosition;
        vertices[1] = transform.localPosition + new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)) * _radius;
        vertices[2] = transform.localPosition + new Vector3(Mathf.Cos(radian), -Mathf.Sin(radian)) * _radius;

        collider.SetPath(0, vertices);
    }
}
