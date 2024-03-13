using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Client_", menuName = "Client/Client", order = 0)]
public class ClientSo : ScriptableObject
{
    /** PROPERTIES */
    public string ClientName => _clientName;
    public ClientConfigSo ClientConfig => _clientConfig;
    
    
    /** FIELDS */

    [SerializeField, Header("Infos")] 
    private string _clientName;

    [SerializeField, Header("Type")]
    private ClientConfigSo _clientConfig;
    
}
