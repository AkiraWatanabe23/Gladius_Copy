using Constants;
using UnityEngine;

/// <summary> チャージビーム </summary>
public class ChargeBeamBullet : IBulletData
{
    private int _defaultAttackValue = int.MinValue;

    public GameObject BulletObj { get; set; }
    public Transform Transform { get; set; }
    public float Speed { get; set; }
    public int AttackValue { get; set; }
    public int GunnerLayer { get; set; }
    public Vector2 MoveForward { get; set; }
    public Rigidbody2D Rb2d { get; set; }

    public void Movement()
    {
        Rb2d.velocity = MoveForward * Speed;
    }

    public void Hit(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out IDamageable damageTarget)) { return; }

        damageTarget.ReceiveDamage(AttackValue);
        GameManager.Instance.ObjectPool.RemoveObject(BulletObj);
    }

    /// <summary> 攻撃時に値を更新する </summary>
    /// <param name="updateMagnification"> 更新する倍率 </param>
    public void BeamDataSetting(int updateMagnification = 1)
    {
        if (updateMagnification <= 0) { updateMagnification = 1; }
        //初期値が未設定であれば設定する
        if (_defaultAttackValue <= 0) { _defaultAttackValue = AttackValue; }

        Consts.Log($"ChargeBeam : AttackValue *= {updateMagnification}");
        AttackValue *= updateMagnification;
        AttackValue = _defaultAttackValue;
    }
}
