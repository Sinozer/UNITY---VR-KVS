using System;
using TMPro;
using UnityEngine;

public class TextAndPriceUI : MonoBehaviour
{
    public int ProductIndex => _productIndex;
    public ItemSo Product => _productSo;
    
    
    [SerializeField] private TextMeshProUGUI _contentTitleText;
    [SerializeField] private TextMeshProUGUI _productPriceText;
    
    private int _productIndex;
    private ItemSo _productSo;
    
    public event Action<int, ItemSo> OnRemovedProduct;

    
    public void Initialize(ItemSo product, int productGlobalIndex)
    {
        _productSo = product;
        _productIndex = productGlobalIndex;
    }
    
    public void RemoveProduct()
    {
        OnRemovedProduct?.Invoke(_productIndex, _productSo);
    }
    
    public void SetTextAndPrice(string productName, float productPrice)
    {
        SetText(productName);
        SetPrice(productPrice);
    }

    public void SetText(string textName)
    {
        _contentTitleText.text = textName;
    }

    public void SetPrice(float price)
    {
        _productPriceText.text = "$ " + price.ToString("0.00");
    }

    public void ResetAll()
    {
        ResetName();
        ResetPrice();
    }

    public void ResetPrice()
    {
        SetPrice(0);
    }

    public void ResetName()
    {
        SetText("");
    }
}
