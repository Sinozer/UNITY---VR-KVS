using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ProductListUI : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup _productListLayout;
    [SerializeField, AssetsOnly] private ProductInfoUI _productInfoPrefab;
    
    
    public void AddItemInfoToList(ItemSo product)
    {
        ProductInfoUI productInfo = Instantiate(_productInfoPrefab.gameObject, _productListLayout.transform)
            .GetComponent<ProductInfoUI>();
        productInfo.SetProductInfo(product.ItemName, product.ItemPrice);
    }
}
