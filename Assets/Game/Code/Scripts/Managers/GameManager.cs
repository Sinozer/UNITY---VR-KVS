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
    
    [Header("Lights")]
    [SerializeField] private SwitchLights _switchLights;
    [SerializeField, Range(10, 600)] private float _minTimerBeforeSwitch = 60;
    [SerializeField, Range(10, 600)] private float _maxTimerBeforeSwitch = 120;

    
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
        
        StartLightsSwitchTimer();
    }

    public void UpdateQuota(float moneyToAdd, float clientSatisfaction)
    {
        AddMoneyQuota(moneyToAdd);
        UpdateClientSatisfaction(clientSatisfaction);
        
        UIManager.Instance.HideClientTracker();
    }

    private void AddMoneyQuota(float moneyToAdd)
    {
        _currentMoneyQuota += moneyToAdd;
        UpdateMoneyQuotaUI();
    }

    private void UpdateClientSatisfaction(float clientSatisfaction)
    {
        clientSatisfaction = Mathf.Clamp(clientSatisfaction, 0, 100);
        
        float satisfactionMultiplier = Mathf.Clamp((clientSatisfaction - 50) / 50, -1, 1);
        _currentClientSatisfaction += _maxSatisfactionToAdd * satisfactionMultiplier;
        
        Debug.Log(_currentClientSatisfaction);

        UpdateClientSatisfactionUI();
    }
    
    public void TurnLightsOn()
    {
        _switchLights.TurnLightsOn();
        StartLightsSwitchTimer();
    }

    private void UpdateMoneyQuotaUI()
    {
        OnMoneyQuotaUpdated?.Invoke(_currentMoneyQuota, _targetMoneyQuota);
    }
    
    private void UpdateClientSatisfactionUI()
    {
        OnClientSatisfactionUpdated?.Invoke(_currentClientSatisfaction, MAX_CLIENT_SATISFACTION);
    }

    private void LightsOff() => _switchLights.TurnLightsOff();
    
    private void StartLightsSwitchTimer()
    {
        float timer = UnityEngine.Random.Range(_minTimerBeforeSwitch, _maxTimerBeforeSwitch);
        Invoke(nameof(LightsOff), timer);
    }
}