using System;
using System.Collections;
using Game.Code.Scripts;
using Game.Code.Scripts.UI;
using UnityEngine;

public class FurnitureScanner : MonoBehaviour
{    
    [Header("Events")]
    [SerializeField] private ItemEventSO _onProductScanned;
    
    [SerializeField] private AudioSource _audioSource;
    
    
    [Header("Settings")]
    [SerializeField] private float _cooldown = 0.1f;

    private bool _isInCooldown = false;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isInCooldown) return;
        
        if (other.TryGetComponent(out Furniture product))
        {
            if (!product.ProductSo)
            {
                Debug.LogError("Product doesn't contain the SO");
                return;
            }
            
            AudioSource.PlayClipAtPoint(_audioSource.clip, transform.position);
            
            _onProductScanned.RaiseEvent(product.ProductSo);
            StartCoroutine(CooldownEnumerator());
        }
    }

    private IEnumerator CooldownEnumerator()
    {
        _isInCooldown = true;
        yield return new WaitForSeconds(_cooldown);
        _isInCooldown = false;
    }
}
