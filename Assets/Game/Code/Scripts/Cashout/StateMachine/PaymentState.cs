using System;
using System.Collections;
using FiniteStateMachine;
using UnityEngine;

public class PaymentState : BaseState
{
    public event Action OnClientFinished;
    
    [Header("Client rotation")]
    [SerializeField] private AnimationCurve _clientRotationCurve;
    [SerializeField, Range(-180, 180)] private float _rotationAngle;
    [SerializeField, Range(0, 10)] private float _delayBeforeWalkingToPayment;
    
    [Header("Client Move")]
    [SerializeField, Range(0, 5)] private float _clientSpeed;
    [SerializeField, Range(0, 1)] private float _paymentWaypointAcceptanceRadius;
    
    public override void Enter(StateMachine stateMachine, params object[] args)
    {
        base.Enter(stateMachine, args);
        _currentClient = args[0] as ClientBehavior;
        _paymentPoint = args[1] as Transform;

        StartCoroutine(ClientCoroutine());

        return;
        
        IEnumerator ClientCoroutine()
        {
            yield return RotateClient(false);

            yield return new WaitForSeconds(_delayBeforeWalkingToPayment);

            yield return WalkToPayment();

            yield return RotateClient(true);

            // Register that the player registered the amount to pay
            
            // If the player did register, pay and leave
        }
    }
    
    private ClientBehavior _currentClient;
    private Transform _paymentPoint;
    
    private IEnumerator RotateClient(bool shouldFaceCashOut)
    {
        float time = 0.0f;
        float duration = _clientRotationCurve.keys[_clientRotationCurve.length - 1].time;
        float angleA = shouldFaceCashOut ? 0.0f : _rotationAngle;
        float angleB = shouldFaceCashOut ? _rotationAngle : 0.0f;

        while (time < duration)
        {
            float rotationAngle = Mathf.Lerp(angleA, angleB, _clientRotationCurve.Evaluate(time));
            _currentClient.transform.rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
            Debug.Log("Rotating client to " + rotationAngle);

            time += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator WalkToPayment()
    {
        float step = _clientSpeed * Time.deltaTime;
        while (Vector3.Distance(_currentClient.transform.position, _paymentPoint.position) > _paymentWaypointAcceptanceRadius)
        {
            _currentClient.transform.position = Vector3.MoveTowards(_currentClient.transform.position, _paymentPoint.position, step);
            yield return null;
        }
    }
}