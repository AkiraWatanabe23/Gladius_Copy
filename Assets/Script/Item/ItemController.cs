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
        Destroy(gameObject);
    }
}
