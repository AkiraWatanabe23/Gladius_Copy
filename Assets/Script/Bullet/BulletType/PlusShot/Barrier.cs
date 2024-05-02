using UnityEngine;

public class Barrier : IBulletData
{
    [Tooltip("効果時間")]
    [SerializeField]
    private float _effectTime = 1f;
    [Tooltip("耐久回数")]
    [SerializeField]
    private int _enduranceCount = 5;

    private float _effectTimer = 0f;
    /// <summary> 耐久回数をキャッシュしておく（オブジェクトを再利用したときに齟齬が起きないように） </summary>
    private int _initEnduranceCount = -1;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        if (_effectTimer >= _effectTime) { Reset(); }

        _effectTimer += Time.deltaTime;
    }

    public void Hit(Collider2D collision)
    {
        if (_initEnduranceCount < 0) { _initEnduranceCount = _enduranceCount; }

        if (collision.gameObject.TryGetComponent(out IDamageable _) ||
            collision.gameObject.TryGetComponent(out BulletController _))
        {
            GameManager.Instance.ObjectPool.RemoveObject(collision.gameObject);

            _enduranceCount--;
            if (_enduranceCount <= 0) { Reset(); }
        }
    }

    private void Reset()
    {
        _effectTimer = 0f;
        _enduranceCount = _initEnduranceCount;
        GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
    }
}
