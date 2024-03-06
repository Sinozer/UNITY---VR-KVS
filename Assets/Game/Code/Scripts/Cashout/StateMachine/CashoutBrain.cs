using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class CashoutBrain : StateMachine
{
    [SerializeField] private GameObject _clientPrefab;
    [SerializeField, ReadOnly] private List<ClientBehavior> _clients;
    [SerializeField, ReadOnly] private ClientBehavior _currentClient;
    [SerializeField, Range(0, 5)] private float _spawnRate;
    private Coroutine _spawnCoroutine;
    
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _itemDeliveryPoint;
    [SerializeField] private Transform _paymentPoint;

    [SerializeField] private BaseState _waitingState;
    [SerializeField] private BaseState _spawningItemsState;
    [SerializeField] private BaseState _paymentState;
    
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
            _currentClient = Instantiate(_clientPrefab, spawnPoint, Quaternion.identity).GetComponent<ClientBehavior>();
            yield return new WaitForSeconds(_spawnRate);
        }
    }
}