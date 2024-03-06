using UnityEngine;

public interface IBulletData
{
    public GameObject BulletObj { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    /// <summary> 自身を撃ったオブジェクトのLayer </summary>
    public LayerMask GunnerLayer { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    /// <summary> 初期化関数 </summary>
    public void Init(GameObject go, float speed, int attackValue, LayerMask gunner)
    {
        BulletObj = go;
        Speed = speed;
        AttackValue = attackValue;
        GunnerLayer = gunner;

        if (go.TryGetComponent(out Rigidbody2D rb2d)) { Rb2d = rb2d; }
        else { Rb2d = go.AddComponent<Rigidbody2D>(); }

        Rb2d.isKinematic = true;
        Rb2d.gravityScale = 0f;
    }

    public void Movement();

    /// <summary> オブジェクトとの衝突時に実行される関数 </summary>
    public void Hit(GameObject hitTarget);
}
