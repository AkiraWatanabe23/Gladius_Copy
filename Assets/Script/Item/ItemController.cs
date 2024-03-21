using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SubclassSelector]
    [SerializeReference]
    private IItemSystem _itemSystem = default;

    public IItemSystem ItemSystem => _itemSystem;
}
