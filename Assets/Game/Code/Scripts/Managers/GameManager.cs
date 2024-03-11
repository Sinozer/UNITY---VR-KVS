using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static ItemRegistry ItemRegistry => Instance._itemRegistry;
    
    
    [Header("Managers")]
    [SerializeField] private ItemRegistry _itemRegistry;
    
    [Header("Player")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerSpawnPoint;
    
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        _player.transform.position = _playerSpawnPoint.transform.position;
    }
}