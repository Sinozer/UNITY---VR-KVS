using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class ClientHolder : MonoBehaviour
{
    public ClientBehavior CurrentClient => _currentClient;
    
    [SerializeField] private GameObject _clientPrefab;
    [SerializeField, Range(5, 120)] private float _spawnRate;
    
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
        Destroy(_currentClient.gameObject);
        
        _currentClient = null;
        
        if (_waitingClients.Count > 0)
        {
            _currentClient = _waitingClients.Dequeue();
        }
    }
        
    private Coroutine _spawnCoroutine;
    private Transform _spawnPoint;
        
    private IEnumerator SpawnClients()
    {
        while (true)
        {
            Vector3 spawnPoint = _spawnPoint.position;
            ClientBehavior client = Instantiate(_clientPrefab, spawnPoint, Quaternion.identity).GetComponent<ClientBehavior>();
            
            _currentClient = _waitingClients.Count == 0 ? client : _currentClient;
            _waitingClients.Enqueue(client);
            
            yield return new WaitForSeconds(_spawnRate);
        }
    }
}