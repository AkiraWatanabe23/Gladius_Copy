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

    private int _spawnCounter = 0;

    public int ItemSpawnMultiple => _itemSpawnMultiple;

    public void Spawn(Transform spawnPos)
    {
        if (_itemPrefabs == null) { Debug.Log("no item settings"); return; }
        if (_spawnCounter + 1 >= _itemPrefabs.Count) { _spawnCounter = 0; }

        _spawnCounter++;
        var spawnItem = Instantiate(_itemPrefabs[_spawnCounter - 1], spawnPos.position, Quaternion.identity);
        if (spawnItem.TryGetComponent(out ItemController item)) { item.ItemSystem.Initialize(); }
    }
}
