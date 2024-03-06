using UnityEngine;

public class Common : MonoBehaviour
{
    public ObjectPool ObjectPool { get; private set; }

    public static Common Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }

        ObjectPool = new();
    }
}
