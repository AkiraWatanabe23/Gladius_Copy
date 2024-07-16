using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player.HitTrigger(collision);
    }
}
