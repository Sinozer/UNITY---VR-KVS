using System;
using Game.Code.Scripts;
using UnityEngine;

public class FurnitureScanner : MonoBehaviour
{    
    [SerializeField] private AudioSource _audioSource;
    
    public event Action<ItemSo> OnItemScanned;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FurnitureBehavior product))
        {
            Debug.Log("bip");
            _audioSource.Play();
            /*
            OnItemScanned?.Invoke(item);
            */
        }
    }
}
