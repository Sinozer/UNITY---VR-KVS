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
    private float _globalClientSatisfaction;
    
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
        _globalClientSatisfaction = 50;
        
        UpdateMoneyQuotaUI();
        UpdateClientSatisfactionUI();
    }

    public void UpdateQuota(float moneyToAdd, float baseSatisfaction, float clientSatisfaction)
    {
        AddMoneyQuota(moneyToAdd);
        UpdateClientSatisfaction(baseSatisfaction, clientSatisfaction);
        
        UIManager.Instance.HideClientTracker();
    }

    private void AddMoneyQuota(float moneyToAdd)
    {
        _currentMoneyQuota += moneyToAdd;
        UpdateMoneyQuotaUI();
    }

    private void UpdateClientSatisfaction(float baseSatisfaction, float clientSatisfaction)
    {
        Debug.Log(clientSatisfaction);
        clientSatisfaction = Mathf.Clamp(clientSatisfaction, 0, MAX_CLIENT_SATISFACTION);
        
        float satisfactionMultiplier = Mathf.Clamp((clientSatisfaction - baseSatisfaction * 0.5f) / baseSatisfaction * 0.5f, -1, 1);
        _globalClientSatisfaction += _maxSatisfactionToAdd * satisfactionMultiplier;
        
        Debug.Log(_globalClientSatisfaction);

        UpdateClientSatisfactionUI();
    }

    private void UpdateMoneyQuotaUI()
    {
        OnMoneyQuotaUpdated?.Invoke(_currentMoneyQuota, _targetMoneyQuota);
    }
    
    private void UpdateClientSatisfactionUI()
    {
        OnClientSatisfactionUpdated?.Invoke(_globalClientSatisfaction, MAX_CLIENT_SATISFACTION);
    }
}