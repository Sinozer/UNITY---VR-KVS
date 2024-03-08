using System;
using System.Collections;
using FiniteStateMachine;
using Game.Code.Scripts.Extensions;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class SpawningItemsState : BaseState
{
    public event Action OnClientItemsSpawned;

    [SerializeField] private AnimationCurve _clientRotationCurve;
    [SerializeField, Range(-180, 180)] private float _rotationAngle;

    [SerializeField, Range(0, 10)] private float _delayBeforeItemSpawn;
    [SerializeField, Range(0, 10)] private float _delayBeforeMovingToPayment;
    [SerializeField, Range(0, 10)] private float _delayBeforeWalkingToPayment;

    public override void Enter(StateMachine stateMachine, params object[] args)
    {
        base.Enter(stateMachine, args);
        _currentClient = args[0] as ClientBehavior;
        _spawnArea = args[1] as BoxCollider;

        StartCoroutine(ClientCoroutine());

        return;
        
        IEnumerator ClientCoroutine()
        {
            yield return RotateClient();

            yield return new WaitForSeconds(_delayBeforeItemSpawn);

            yield return SpawnClientItem();

            yield return new WaitForSeconds(_delayBeforeMovingToPayment);

            OnClientItemsSpawned?.Invoke();
        }
    }

    private ClientBehavior _currentClient;
    private BoxCollider _spawnArea;

    private IEnumerator RotateClient()
    {
        float time = 0.0f;
        float duration = _clientRotationCurve.keys[_clientRotationCurve.length - 1].time;

        while (time < duration)
        {
            float rotationAngle = Mathf.Lerp(0.0f, _rotationAngle, _clientRotationCurve.Evaluate(time));
            _currentClient.transform.rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);

            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SpawnClientItem()
    {
        foreach (var item in _currentClient.ShoppingList)
        {
            for (int i = 0; i < item.Value; i++)
            {
                Vector3 spawnPoint = _spawnArea.RandomPointInBounds();
                GameObject _ = Instantiate(item.Key.Prefab, spawnPoint, Quaternion.identity);
                yield return new WaitForSeconds(_currentClient.DelayBetweenItems);
            }
        }
        yield return null;
    }
}