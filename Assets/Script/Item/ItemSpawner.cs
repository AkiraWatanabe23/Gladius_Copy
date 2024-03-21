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

    private int _itemSpawnCount = 0;

    public int ItemSpawnCount
    {
        get => _itemSpawnCount;
        set
        {
            _itemSpawnCount = value;
            if (IsSpawn) { ItemSpawn(); }
        }
    }

    protected int ItemIndex => _itemSpawnCount % _itemSpawnMultiple;
    protected bool IsSpawn => ItemIndex == 0;

    private void ItemSpawn()
    {
        var index = ItemIndex;
        if (_itemPrefabs == null) { Debug.Log("no item settings"); return; }
        if (index + 1 >= _itemPrefabs.Count) { index = 0; }

        var spawnItem = Instantiate(_itemPrefabs[index]);
        if (spawnItem.TryGetComponent(out ItemController item)) { item.ItemSystem.Initialize(); }
    }
}
