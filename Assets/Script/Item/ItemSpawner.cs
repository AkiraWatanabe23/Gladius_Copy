using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Min(1)]
    [Range(0, 10)]
    [Tooltip("アイテム生成の倍数（n体敵を倒すごとに生成される）")]
    [SerializeField]
    private int _itemSpawnMultiple = 5;
    [Tooltip("順に生成されるアイテム")]
    [SerializeField]
    private List<GameObject> _itemPrefabs = default;

    public int ItemSpawnMultiple => _itemSpawnMultiple;

    public void Spawn(int index)
    {
        if (_itemPrefabs == null) { Debug.Log("no item settings"); return; }
        if (index + 1 >= _itemPrefabs.Count) { index = 0; }

        var spawnItem = Instantiate(_itemPrefabs[index]);
        if (spawnItem.TryGetComponent(out ItemController item)) { item.ItemSystem.Initialize(); }
    }
}
