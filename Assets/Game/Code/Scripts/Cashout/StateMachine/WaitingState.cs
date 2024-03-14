using System;
using FiniteStateMachine;
using UnityEngine;

public class WaitingState : BaseState
{
    public event Action OnClientArrivedAtDeliveryPoint;
    
    [SerializeField, Range(0, 5)] private float _clientSpeed;
    [SerializeField, Range(0,10)] private float _acceptanceRadius;
    public override void Enter(StateMachine stateMachine, params object[] args)
    {
        base.Enter(stateMachine, args);
        _currentClient = args[0] as ClientBehavior;
        _itemDeliveryPoint = args[1] as Transform;
        
        if (_currentClient != null)
        {
            UIManager.Instance.UpdateClientTracker(_currentClient.client);
        }
    }

    private ClientBehavior _currentClient;
    private Transform _itemDeliveryPoint;

    private void Update()
    {
        if (_currentClient == null) return;
        
        _currentClient.transform.position =
            Vector3.MoveTowards(_currentClient.transform.position, _itemDeliveryPoint.position, _clientSpeed * Time.deltaTime);

        if (Vector3.Distance(_currentClient.transform.position, _itemDeliveryPoint.position) < _acceptanceRadius)
        {
            OnClientArrivedAtDeliveryPoint?.Invoke();
        }
    }
}