using System;
using UnityEngine;

[Serializable]
public class BulletHolder
{
    [field: SerializeField]
    public GameObject DefaultBullet { get; private set; }
}

public class EnemyCommon : MonoBehaviour
{
    [SerializeField]
    private bool _debug = true;

    [field: SerializeField]
    public GameObject EnemyCorePrefab { get; private set; }
    [field: SerializeField]
    public BulletHolder BulletHolder { get; private set; } = new();

    public ObjectPool ObjectPool { get; private set; }

    public static EnemyCommon Instance { get; private set; }

    private void Awake()
    {
        if (!_debug) { return; }

        if (Instance == null) { Instance = this; }
        if (ObjectPool == null) { ObjectPool = new(); }
    }
}
