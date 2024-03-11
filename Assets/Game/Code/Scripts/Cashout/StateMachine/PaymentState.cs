using System;
using System.Collections;
using FiniteStateMachine;
using UnityEngine;

public class PaymentState : BaseState
{
    public event Action OnClientFinished;

    [Header("Client rotation")] [SerializeField]
    private AnimationCurve _clientRotationCurve;

    [SerializeField, Range(-180, 180)] private float _rotationAngle;
    [SerializeField, Range(0, 10)] private float _delayBeforeWalkingToPayment;

    [Header("Client Move")] [SerializeField, Range(0, 5)]
    private float _clientSpeed;

    [SerializeField, Range(0, 1)] private float _paymentWaypointAcceptanceRadius;

    public override void Enter(StateMachine stateMachine, params object[] args)
    {
        base.Enter(stateMachine, args);
        _currentClient = args[0] as ClientBehavior;
        _paymentPoint = args[1] as Transform;
        _leavePoint = args[2] as Transform;

        StartCoroutine(ClientCoroutine());

        return;

        IEnumerator ClientCoroutine()
        {
            var brain = stateMachine as CashoutBrain;
            brain!.OnTotalPriceRegistered += OnTotalPriceRegistered;

            yield return RotateClient(false);

            yield return new WaitForSeconds(_delayBeforeWalkingToPayment);

            yield return WalkTo(_paymentPoint.position);

            yield return RotateClient(true);
        }
    }

    private ClientBehavior _currentClient;
    private Transform _paymentPoint;
    private Transform _leavePoint;

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

            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator WalkTo(Vector3 to)
    {
        float step = _clientSpeed * Time.deltaTime;
        while (Vector3.Distance(_currentClient.transform.position, to) > _paymentWaypointAcceptanceRadius)
        {
            _currentClient.transform.position = Vector3.MoveTowards(_currentClient.transform.position, to, step);
            yield return null;
        }
    }

    private void OnTotalPriceRegistered(float totalPrice)
    {
        _currentClient.PrepareToLeave(totalPrice);

        OnClientFinished?.Invoke();

        StartCoroutine(LeaveCoroutine());

        return;

        IEnumerator LeaveCoroutine()
        {
            yield return RotateClient(false);

            yield return WalkOut();
        }

        IEnumerator WalkOut()
        {
            float startTime = Time.time;
            float duration = 10f;
            Vector3 startPosition = _currentClient.transform.position;
            Vector3 destination = _leavePoint.position;

            while (Time.time <= startTime + duration)
            {
                yield return null;

                if (_currentClient == null) yield break;
                
                _currentClient.transform.position =
                    Vector3.Lerp(startPosition, destination, (Time.time - startTime) / duration);
            }
        }
    }
}