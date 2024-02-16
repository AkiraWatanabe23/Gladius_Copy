using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ObjectPool ObjectPool { get; private set; }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }

        if (ObjectPool == null) { ObjectPool = new(); }
    }
}
