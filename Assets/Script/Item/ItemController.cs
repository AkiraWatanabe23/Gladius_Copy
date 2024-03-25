using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SubclassSelector]
    [SerializeReference]
    private IGameItem _itemSystem = default;

    public IGameItem ItemSystem => _itemSystem;
}
