using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class CashoutBrain : StateMachine
{
    [Header("Clients")]
    [SerializeField] private GameObject _clientPrefab;
    [OdinSerialize, ReadOnly] private Queue<ClientBehavior> _waitingClients;
    [SerializeField, ReadOnly] private ClientBehavior _currentClient;
    [SerializeField, Range(0, 5)] private float _spawnRate;
    private Coroutine _spawnCoroutine;
    
    [Header("Waypoints")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _itemDeliveryPoint;
    [SerializeField] private Transform _paymentPoint;

    [Header("State Machine")]
    [SerializeField] private BaseState _spawningItemsState;
    [SerializeField] private BaseState _paymentState;
    
    [Header("Door")]
    [SerializeField] private GameObject _door;
    
    public void StopSpawning()
    {
        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
    }
        
    private void Start()
    {
        _spawnCoroutine = StartCoroutine(SpawnClients());
    }
    
    private IEnumerator SpawnClients()
    {
        while (true)
        {
            Vector3 spawnPoint = _spawnPoint.position;
            ClientBehavior client = Instantiate(_clientPrefab, spawnPoint, Quaternion.identity).GetComponent<ClientBehavior>();
            _waitingClients.Enqueue(client);
            yield return new WaitForSeconds(_spawnRate);
        }
    }
}