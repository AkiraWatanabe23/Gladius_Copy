using UnityEngine;

public class Melee : IBulletData
{
    [Tooltip("何秒間生きてるか")]
    [SerializeField]
    private float _effectLife = 5f;
    [Tooltip("回転するときのPlayerとの距離")]
    [SerializeField]
    private float _rotateDistance = 1f;

    private float _angle = 0f;
    private float _effectTimer = 0f;
    private Transform _rotateCenter = default;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public int GunnerLayer { get; set; }
    public Vector2 MoveForward { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        if (_effectTimer >= _effectLife) { Reset(); }

        GameManager.Instance.Melees ??= new();
        if (!GameManager.Instance.Melees.Contains(this)) { GameManager.Instance.Melees.Add(this); }

        _effectTimer += Time.deltaTime;

        _rotateCenter ??= GameManager.Instance.PlayerTransform;
        _angle += Speed * Time.deltaTime;
        // ラジアンに変換する
        float angleRad = _angle * Mathf.Deg2Rad;

        // 新しい位置を計算する
        Vector3 newPos = new(
            _rotateCenter.position.x + Mathf.Cos(angleRad) * _rotateDistance,
            _rotateCenter.position.y + Mathf.Sin(angleRad) * _rotateDistance,
            Transform.position.z
        );

        // オブジェクトを新しい位置に移動させる
        Transform.position = newPos;
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
