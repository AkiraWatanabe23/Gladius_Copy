using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player = default;

    public PlayerController Player
    {
        get
        {
            if (_player == null) { _player = FindObjectOfType<PlayerController>(); }
            return _player;
        }
    }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
}
