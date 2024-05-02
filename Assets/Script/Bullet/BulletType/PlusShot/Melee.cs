using UnityEngine;

public class Melee : IBulletData
{
    [Tooltip("何秒間生きてるか")]
    [SerializeField]
    private float _effectLife = 5f;
    [Tooltip("回転するときのPlayerとの距離")]
    [SerializeField]
    private float _rotateDistance = 1f;

    private float _effectTimer = 0f;
    private Transform _rotateCenter = default;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public LayerMask GunnerLayer { get; set; }
    public Vector2 MoveDirection { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        if (_effectTimer >= _effectLife) { Reset(); }

        GameManager.Instance.Melees ??= new();
        if (!GameManager.Instance.Melees.Contains(this)) { GameManager.Instance.Melees.Add(this); }

        _effectTimer += Time.deltaTime;
        _rotateCenter ??= GameManager.Instance.PlayerTransform;

        Transform.RotateAround(_rotateCenter.position, Vector3.forward, 360f / 2f * Time.deltaTime);
    }

    public void Hit(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

        damageTarget.ReceiveDamage(AttackValue);
        Reset();
    }

    private void Reset()
    {
        _effectTimer = 0f;
        GameManager.Instance.Melees.Remove(this);

        GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
    }
}
