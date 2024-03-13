using System;
using Game.Code.Scripts;
using Game.Code.Scripts.UI;
using UnityEngine;

public class FurnitureScanner : MonoBehaviour
{    
    [Header("Events")]
    [SerializeField] private ItemEventSO _onProductScanned;
    
    [SerializeField] private AudioSource _audioSource;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Furniture product))
        {
            if (!product.ProductSo)
            {
                Debug.LogError("Product doesn't contain the SO");
                return;
            }
            
            Debug.Log("Product " + product.ProductSo.ItemName + " scanned");
            AudioSource.PlayClipAtPoint(_audioSource.clip, transform.position);
            
            _onProductScanned.RaiseEvent(product.ProductSo);
        }
    }
}
