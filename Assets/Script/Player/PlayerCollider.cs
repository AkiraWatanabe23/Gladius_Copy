using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"HIt {collision.gameObject.name}");
        _player.HitTrigger(collision);
    }
}
