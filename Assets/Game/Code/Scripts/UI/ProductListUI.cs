using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ProductListUI : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _productListLayout;
    [SerializeField, AssetsOnly] private TextAndPriceUI textAndPricePrefab;

    public event Action OnProductListChanged;

    
    public void AddItemInfoToList(ItemSo product)
    {
        TextAndPriceUI textAndPrice = Instantiate(textAndPricePrefab.gameObject, _productListLayout.transform)
            .GetComponent<TextAndPriceUI>();
        textAndPrice.SetTextAndPrice(product.ItemName, product.ItemPrice);
        
        OnProductListChanged?.Invoke();
    }

    public void ResetRegisteredProducts()
    {
        foreach (Transform child in _productListLayout.transform)
        {
            Destroy(child);
        }
        
        OnProductListChanged?.Invoke();
    }
}
