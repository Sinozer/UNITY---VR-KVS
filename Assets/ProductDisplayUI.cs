using TMPro;
using UnityEngine;

public class ProductDisplayUI : MonoBehaviour
{
    [SerializeField] private ProductInfoUI _productInfoUI;
    
    
    public void UpdateLastProductInfo(ItemSo product)
    {
        _productInfoUI.SetProductInfo(product.ItemName, product.ItemPrice);
    }
}
