using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, IPause
{
    [SerializeField]
    private PlayerMovement _movement = new();
    [SerializeField]
    private PlayerAttack _attack = new();
    [SerializeField]
    private PlayerHealth _health = new();

    public PlayerMovement Movement => _movement;
    public PlayerAttack Attack => _attack;
    public PlayerHealth Health => _health;

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

    public void HitTrigger(Collider2D collision)
    {
        _health.OnTriggerEnter2D(collision);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        _attack.OnDrawGizmos(gameObject);
    }
#endif

    public void ReceiveDamage(int value) => _health.ReceiveDamage(value);

    public void Pause()
    {
        _movement.Pause();
        _attack.Pause();
    }

    public void Resume()
    {
        _movement.Resume();
        _attack.Resume();
    }
}
