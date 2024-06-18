using UnityEngine;

public class Support : IBulletData
{
    [SerializeField]
    private Transform _laserMuzzle = default;

    private Transform _player = default;
    /// <summary> 自分が何番目の補助兵装か </summary>
    private int _supportIndex = -1;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public int GunnerLayer { get; set; }
    public Vector2 MoveForward { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        _player ??= GameManager.Instance.PlayerTransform;
        if (_supportIndex <= 0)
        {
            _supportIndex = GameManager.Instance.CurrentSupportCount;
            GameManager.Instance.Supports ??= new();
            GameManager.Instance.Supports.Add(this);
        }

        var offset = _player.position - Transform.position;
        //playerとの位置差が一定以上離れたら動く
        MoveForward =
            offset.sqrMagnitude >= GameManager.Instance.SupportMoveSqrtDistance * _supportIndex ?
            offset : Vector2.zero;
        Rb2d.velocity = MoveForward * Speed;
    }

    public void Hit(Collider2D collision) { }

    public void Attack()
    {
        var bullet = GameManager.Instance.ObjectPool.SpawnObject(
            GameManager.Instance.BulletHolder.BulletsDictionary[InitialBulletType.Laser]);

        bullet.transform.position = _laserMuzzle.position;
        var bulletData = bullet.GetComponent<BulletController>();
        bulletData.Initialize(AttackValue, GunnerLayer, Vector2.right);
    }
}
