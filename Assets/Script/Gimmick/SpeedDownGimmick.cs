using UnityEngine;

/// <summary> 速度減少 </summary>
public class SpeedDownGimmick : MonoBehaviour
{
    [Tooltip("何回弾に撃たれたら消えるか")]
    [SerializeField]
    private int _gimmickLife = 1;
    [Tooltip("速度減少値（現在の速度から引かれる値）")]
    [SerializeField]
    private int _speedDownValue = 1;

    private Rigidbody2D _rb2d = default;

    protected int GimmickLife
    {
        get => _gimmickLife;
        private set
        {
            _gimmickLife = value;
            if (_gimmickLife <= 0) { GameManager.Instance.ObjectPool.RemoveObject(gameObject); }
        }
    }

    private void Start()
    {
        if (!gameObject.TryGetComponent(out _rb2d)) { _rb2d = gameObject.AddComponent<Rigidbody2D>(); }
        _rb2d.isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            AudioManager.Instance.PlaySE(SEType.SpeedDown);
            player.Movement.SpeedDown(_speedDownValue);
            GimmickLife = 0;
        }
        else if (collision.gameObject.TryGetComponent(out BulletController _))
        {
            GimmickLife--;
        }
    }
}
