using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class ClientHolder : SerializedMonoBehaviour
{
    public ClientBehavior CurrentClient
    {
        get => _currentClient;
        set
        {
            _currentClient = value;
            
            if (_currentClient == null)
                return;
            
            // Get the player UI to display the forgotten item if there is one
        }
    }

    private ClientBehavior _previousClient;
    
    [SerializeField] private GameObject _clientPrefab;
    [SerializeField, Range(5, 120)] private float _spawnRate;
    [SerializeField] private List<ClientSo> _clientsConfig;
    
    [Header("Debug")]
    [OdinSerialize, ReadOnly, ShowInInspector] private Queue<ClientBehavior> _waitingClients;
    [SerializeField, ReadOnly] private ClientBehavior _currentClient;
        
    public void Initialize(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
        _waitingClients = new Queue<ClientBehavior>();
    }
        
    public void StopActiveCoroutine()
    {
        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
    }
        
    public void StartSpawning()
    {
        StopActiveCoroutine();
            
        _spawnCoroutine = StartCoroutine(SpawnClients());
    }
    
    public void TryDestroyPreviousClient()
    {
        if (_previousClient != null)
        {
            Destroy(_previousClient.gameObject);
        }
    }
        
    private Coroutine _spawnCoroutine;
    private Transform _spawnPoint;
        
    private IEnumerator SpawnClients()
    {
        while (true)
        {
            Vector3 spawnPoint = _spawnPoint.position;
            ClientBehavior client = Instantiate(_clientPrefab, spawnPoint, Quaternion.identity)
                .GetComponent<ClientBehavior>();

            ClientSo clientConfig = _clientsConfig[Random.Range(0, _clientsConfig.Count - 1)];
            client.Initialize(clientConfig);

            if (_waitingClients.Count == 0 && CurrentClient == null)
            {
                CurrentClient = client;
            }
            else
            {
                _waitingClients.Enqueue(client);
            }
            
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    public void TrySelectNextClient()
    {
        _previousClient = CurrentClient;
        CurrentClient = _waitingClients.Count > 0 ? _waitingClients.Dequeue() : null;
    }
}