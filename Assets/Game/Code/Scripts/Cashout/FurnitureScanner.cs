using System;
using Game.Code.Scripts;
using UnityEngine;

public class FurnitureScanner : MonoBehaviour
{    
    [SerializeField] private AudioSource _audioSource;
    
    public event Action<ItemSo> OnItemScanned;

    
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
            OnItemScanned?.Invoke(product.ProductSo);
        }
    }
}
