using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private PlayerMovement _movement = new();
    [SerializeField]
    private PlayerAttack _attack = new();
    [SerializeField]
    private PlayerHealth _health = new();

    private void Start()
    {
        _movement.Initialize(gameObject);
        _attack.Initialize(gameObject);
        _health.Initialize(gameObject);
    }

    private void Update()
    {
        _movement.OnUpdate();
        _attack.OnUpdate();
    }

    public void ReceiveDamage(int value) { _health.ReceiveDamage(value); }
}
