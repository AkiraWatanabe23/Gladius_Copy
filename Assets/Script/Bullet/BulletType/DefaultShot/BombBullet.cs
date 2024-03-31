using UnityEngine;

/// <summary> 爆発 </summary>
public class BombBullet : IBulletData
{
    [SerializeField]
    private float _bombAreaRadius = 1f;
    [SerializeField]
    private GameObject _bombEffect = default;

    private bool _isThrew = false;

    public float BombAreaRadius => _bombAreaRadius;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        if (_isThrew) { return; }

        ThrowBomb();
        _isThrew = true;
    }

    private void ThrowBomb()
    {
        Rb2d.isKinematic = false;
        Rb2d.gravityScale = 1f;

        Vector2 direction = GameManager.Instance.PlayerTransform.position - Transform.position;
        direction.Normalize();

        Rb2d.AddForce(direction * 5f, ForceMode2D.Impulse);
    }

    public void Hit(Collider2D collision)
    {
        //床にぶつかったら爆発
        if (collision.gameObject.TryGetComponent(out Ground _))
        {
            if (_bombEffect == null) { Debug.Log("no assigned"); return; }

            _bombEffect.SetActive(true);
            _bombEffect.GetComponent<CircleCollider2D>().radius = _bombAreaRadius;
        }
    }
}
