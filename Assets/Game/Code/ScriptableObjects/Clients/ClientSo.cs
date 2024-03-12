using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ClientType
{
    CHILL,
    NORMAL,
    RUSH,
    KAREN
}

[CreateAssetMenu(fileName = "Client_", menuName = "ClientsSO", order = 0)]
public class ClientSo : ScriptableObject
{
    /** PROPERTIES */
    
    public string ClientName => _clientName;

    public ClientType ClientType => _clientType;

    public float Timer => _timer;

    public float DelayBetweenItems => _delayBetweenItems;

    public Color ClientColor => _clientColor;
    
    /** FIELDS */

    [SerializeField, Header("Infos")] private string _clientName;

    [SerializeField, OnValueChanged(nameof(OnClientTypeChanged)), Header("Type related")]
    private ClientType _clientType;

    [SerializeField, ReadOnly] private float _timer;

    [SerializeField, ReadOnly] private float _delayBetweenItems;

    [SerializeField, ReadOnly] private Color _clientColor;
    
    /** METHODS */

    private ClientSo() => OnClientTypeChanged();

    private void OnClientTypeChanged()
    {
        _timer = _clientType.GetTimer();
        _delayBetweenItems = _clientType.GetDelayBetweenItems();
        _clientColor = _clientType.GetColor();
    }
}