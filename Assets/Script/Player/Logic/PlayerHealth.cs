using System;
using UnityEngine;

[Serializable]
public class PlayerHealth : PlayerSystemBase
{
    [Header("残機数")]
    [ReadOnly]
    [SerializeField]
    private int _remainingAircraft = 5;
    [Header("残機数の初期値")]
    [SerializeField]
    private int _initialRemainingAircraft = 5;

    public int RemainingCount => _remainingAircraft;
    public int MaxRemainingCount => _initialRemainingAircraft;

    public override void Initialize(GameObject go)
    {
        _remainingAircraft = _initialRemainingAircraft;
    }

    public void ReceiveDamage(int value)
    {
        AudioManager.Instance.PlaySE(SEType.PlayerDamaged);
        _remainingAircraft -= value;
        GameManager.Instance.UIController.OnUpdateAircraft(value);
        if (_remainingAircraft <= 0) { Dead(); }
    }

    public void Heal(int value)
    {
        _remainingAircraft += value;
        if (_remainingAircraft > _initialRemainingAircraft) { _remainingAircraft = _initialRemainingAircraft; }

        GameManager.Instance.UIController.OnUpdateAircraft(_remainingAircraft, true);
    }

    private void Dead()
    {
        AudioManager.Instance.PlaySE(SEType.PlayerCrashed);
        _remainingAircraft = 0;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground _)) { Dead(); }
    }
}
