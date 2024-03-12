using Extensions;
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

[CreateAssetMenu(fileName = "Client_", menuName = "ClientsSO", order = 0)]
public class ClientSo : ScriptableObject
{
    /** PROPERTIES */
    
    public string ClientName => _clientName;
    public ClientType ClientType => _clientType;
    public int TimerInSeconds => _timerInSeconds;
    public float DelayBetweenItems => _delayBetweenItems;
    public float BaseSatisfaction => _baseSatisfaction;
    public ClientHumor BaseHumor => _baseHumor;
    public Color ClientColor => _clientColor;
    
    /** FIELDS */

    [SerializeField, Header("Infos")] private string _clientName;

    [SerializeField, OnValueChanged(nameof(OnClientTypeChanged)), Header("Type related")]
    private ClientType _clientType;

    [SerializeField, ReadOnly] private int _timerInSeconds;
    [SerializeField, ReadOnly] private float _baseSatisfaction;
    [SerializeField, ReadOnly] private ClientHumor _baseHumor;
    [SerializeField, ReadOnly] private float _delayBetweenItems;
    [SerializeField, ReadOnly] private Color _clientColor;
    
    /** METHODS */

    private ClientSo() => OnClientTypeChanged();

    private void OnClientTypeChanged()
    {
        _timerInSeconds = _clientType.GetTimer();
        _delayBetweenItems = _clientType.GetDelayBetweenItems();
        _clientColor = _clientType.GetColor();
        _baseSatisfaction = _clientType.GetBaseSatisfaction();
        _baseHumor = DetermineHumor();
    }

    private ClientHumor DetermineHumor()
    {
        return _baseSatisfaction switch
        {
            >= 70 => ClientHumor.HAPPY,
            < 70 and >= 35 => ClientHumor.INDIFFERENT,
            <= 35 => ClientHumor.ANGRY,
            _ => ClientHumor.INDIFFERENT
        };
    }
}
