using FiniteStateMachine;
using UnityEngine;

public class CashoutBrain : StateMachine
{
    [SerializeField] private WaitingState _waitingState;
    [SerializeField] private SpawningItemsState _spawningItemsState;
    [SerializeField] private PaymentState _paymentState;
    
    [Header("Clients")] 
    [SerializeField] private ClientHolder _clientHolder;

    [Header("Waypoints")] 
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _itemDeliveryPoint;
    [SerializeField] private Transform _paymentPoint;
    
    [Header("Door")] 
    [SerializeField] private GameObject _door;
    
    private void Start()
    {
        _clientHolder.Initialize(_spawnPoint);
        _clientHolder.StartSpawning();
        
        GoBackToWaitingState();
    }
    
    private void GoBackToWaitingState()
    {
        ChangeState(_waitingState, _clientHolder.CurrentClient, _itemDeliveryPoint);
        _waitingState.OnClientArrivedAtDeliveryPoint += OnClientArrivedAtDeliveryPoint;
    }
    
    private void OnClientArrivedAtDeliveryPoint()
    {
        ChangeState(_spawningItemsState, _clientHolder.CurrentClient);
    }
    
    private void OnClientFinished()
    {
        _clientHolder.OnCurrentClientFinished();
        
        GoBackToWaitingState();
        
        _door.SetActive(false);
    }
}