using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


public enum ClientType
{
    CHILL,
    NORMAL,
    RUSH,
    KAREN
}

public enum ClientHumor
{
    HAPPY,
    INDIFFERENT,
    ANGRY
}

[CreateAssetMenu(fileName = "ClientConfig_", menuName = "Client/ClientConfig", order = 0)]
public class ClientConfigSo : ScriptableObject
{
    public int TimerInSeconds => _timerInSeconds;
    public float SpawnDelayBetweenItems => _spawnDelayBetweenItems;
    public int MaxDifferentItems => _maxDifferentItems;
    public int MaxNumberOfSameItems => _maxNumberOfSameItems;
    public float BaseSatisfaction => _baseSatisfaction;
    public float ForgottenItemChance => _forgottenItemChance;
    public float SatisfactionLossOnMissingItem => _satisfactionLossOnMissingItem;
    public float SatisfactionLossOnForgottenItem => _satisfactionLossOnForgottenItem;
    public ClientHumor BaseHumor => _baseHumor;
    public Color ClientColor => _clientColor;
    public ClientType ClientType => _clientType;
    public int MaxHumor => _maxHumor;
    

    [SerializeField] private ClientType _clientType; 
    [SerializeField, MinValue(0)] private int _timerInSeconds;
    
    [Header("Humor")]
    [SerializeField, Range(20, 100), OnValueChanged(nameof(DetermineHumor))] private float _baseSatisfaction;
    [SerializeField, Range(0, 1)] private float _satisfactionLossOnMissingItem = 0.1f; 
    [SerializeField, Range(0, 1)] private float _satisfactionLossOnForgottenItem = 0.3f; 
    [ShowInInspector, ReadOnly] private ClientHumor _baseHumor;
    
    
    [Header("Skin")]
    [SerializeField] private Color _clientColor;
    
    [Header("Items")]
    [SerializeField, Range(0, 1)] private float _forgottenItemChance;
    [SerializeField, MinValue(0)] private float _spawnDelayBetweenItems;
    [SerializeField, MinValue(0)] private int _maxDifferentItems;
    [SerializeField, MinValue(0)] private int _maxNumberOfSameItems;

    private readonly int _maxHumor = Enum.GetValues(typeof(ClientHumor)).Length;

    
    private void Awake()
    {
        _baseHumor = DetermineHumor(BaseSatisfaction);
    }

    public ClientHumor DetermineHumor(float satisfaction)
    {
        return satisfaction switch
        {
            >= 70 => ClientHumor.HAPPY,
            < 70 and >= 35 => ClientHumor.INDIFFERENT,
            <= 35 => ClientHumor.ANGRY,
            _ => ClientHumor.INDIFFERENT
        };
    }
}
