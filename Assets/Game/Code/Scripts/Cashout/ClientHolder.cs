using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public class ClientHolder : MonoBehaviour
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
    
    [SerializeField] private GameObject _clientPrefab;
    [SerializeField, Range(5, 120)] private float _spawnRate;
    [SerializeField] private List<ClientSo> _clientsConfig;
    
    [Header("Debug")]
    [OdinSerialize, ReadOnly] private Queue<ClientBehavior> _waitingClients;
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
    
    public void OnCurrentClientFinished()
    {
        Destroy(CurrentClient.gameObject);
        
        CurrentClient = null;
        
        if (_waitingClients.Count > 0)
        {
            CurrentClient = _waitingClients.Dequeue();
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
            
            CurrentClient = _waitingClients.Count == 0 ? client : _currentClient;       // Check if there is no problem with reassigning the same current client
            _waitingClients.Enqueue(client);
            
            yield return new WaitForSeconds(_spawnRate);
        }
    }
}