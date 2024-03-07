using System.Collections;
using FiniteStateMachine;
using UnityEngine;

public class SpawningItemsState : BaseState
{
    [SerializeField] private AnimationCurve _clientRotationCurve;
    [SerializeField, Range(0,360)] private float _rotationAngle;
    
    public override void Enter(StateMachine stateMachine, params object[] args)
    {
        base.Enter(stateMachine, args);
        _currentClient = args[0] as ClientBehavior;
        
        StartCoroutine(ClientCoroutine());
    }
    
    private IEnumerator ClientCoroutine()
    {
        yield return RotateClient();
        
        yield return new WaitForSeconds(3.0f);
        
        yield return SpawnClientItem();
        
        yield return new WaitForSeconds(3.0f);
        
    }
    
    private ClientBehavior _currentClient;
    
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
        
        
        yield return null;
    }
}