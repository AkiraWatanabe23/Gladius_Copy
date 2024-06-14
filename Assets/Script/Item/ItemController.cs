using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SubclassSelector]
    [SerializeReference]
    private IGameItem _itemSystem = default;

    public IGameItem ItemSystem => _itemSystem;

    /// <summary> 画面外に出たら削除 </summary>
    private void OnBecameInvisible()
    {
        GameManager.Instance.ObjectPool.RemoveObject(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Fan _)) { return; }
        else if (collision.gameObject.TryGetComponent(out PlayerController _))
        {
            _itemSystem.PlayEffect();
            GameManager.Instance.ObjectPool.RemoveObject(gameObject);
        }
    }
}
