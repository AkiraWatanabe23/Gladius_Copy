using UnityEngine;

public class Barrier : IBulletData
{
    [Tooltip("耐久回数")]
    [SerializeField]
    private int _enduranceCount = 5;

    /// <summary> 耐久回数をキャッシュしておく（オブジェクトを再利用したときに齟齬が起きないように） </summary>
    private readonly int _initEnduranceCount = 5;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement() { }

    public void Hit(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable _) ||
            collision.gameObject.TryGetComponent(out BulletController _))
        {
            GameManager.Instance.ObjectPool.RemoveObject(collision.gameObject);

            _enduranceCount--;
            if (_enduranceCount <= 0)
            {
                _enduranceCount = _initEnduranceCount;
                GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
            }
        }
    }
}
