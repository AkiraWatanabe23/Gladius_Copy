using UnityEngine;

public class EnemyCommon : MonoBehaviour
{
    [SerializeField]
    private bool _debug = true;

    [field: SerializeField]
    public GameObject EnemyCorePrefab { get; private set; }

    public static EnemyCommon Instance { get; private set; }

    private void Awake()
    {
        if (!_debug) { return; }

        if (Instance == null) { Instance = this; }
    }
}
