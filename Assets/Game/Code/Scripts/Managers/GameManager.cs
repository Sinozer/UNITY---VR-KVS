using System;
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
    
    [Header("Quota")]
    [SerializeField] private float _targetMoneyQuota = 1000;
    [SerializeField] private float _maxSatisfactionToAdd = 15;

    
    private float _currentMoneyQuota;
    private float _currentClientSatisfaction;
    
    private const float MAX_CLIENT_SATISFACTION = 100;


    public event Action<float, float> OnMoneyQuotaUpdated;
    public event Action<float, float> OnClientSatisfactionUpdated;

    

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

        _currentMoneyQuota = 0;
        _currentClientSatisfaction = 50;
        
        UpdateMoneyQuotaUI();
        UpdateClientSatisfactionUI();
    }

    public void AddMoneyQuota(float moneyToAdd)
    {
        _currentMoneyQuota += moneyToAdd;
        UpdateMoneyQuotaUI();
    }

    public void UpdateClientSatisfaction(float clientSatisfaction)
    {
        float satisfactionToAdd = Mathf.Clamp((clientSatisfaction - 50) / 50, -1, 1);
        _currentClientSatisfaction += satisfactionToAdd;
        UpdateClientSatisfactionUI();
    }

    private void UpdateMoneyQuotaUI()
    {
        OnMoneyQuotaUpdated?.Invoke(_currentMoneyQuota, _targetMoneyQuota);
    }
    
    private void UpdateClientSatisfactionUI()
    {
        OnClientSatisfactionUpdated?.Invoke(_currentClientSatisfaction, MAX_CLIENT_SATISFACTION);
    }
}