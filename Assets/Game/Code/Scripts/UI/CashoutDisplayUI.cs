using System;
using System.Collections.Generic;
using FiniteStateMachine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CashoutDisplayUI : StateMachine
{
    public float EnteredPrice => _paymentView.EnteredPrice;
    
    [Header("States")]
    [SerializeField] private PaymentView _paymentView;
    [SerializeField] private ProductListView _productListView;
    
    [Header("Prefab")]
    [SerializeField, AssetsOnly] private TextAndPriceUI _textAndPricePrefab;
    
    [Header("UI")]
    [SerializeField] private VerticalLayoutGroup _productListLayout;
    [SerializeField] private TextMeshProUGUI _headerText;

    private readonly List<TextAndPriceUI> _productList = new List<TextAndPriceUI>();
    
    public event Action OnProductListChanged;

    
    public void SetProductListView()
    {
        _headerText.text = "Product List";
        ChangeState(_productListView);
        ResetRegisteredProducts();
    }

    public void SetPaymentView(float scannedPrice)
    {
        _headerText.text = "Payment";
        _paymentView.InitNumericalView(scannedPrice);
        ChangeState(_paymentView);
    }
    
    public void AddItemInfoToList(ItemSo product, int index, Action<int> deleteCallback)
    {
        TextAndPriceUI textAndPrice = Instantiate(_textAndPricePrefab.gameObject, _productListLayout.transform)
            .GetComponent<TextAndPriceUI>();
        textAndPrice.Initialize(product, index);
        textAndPrice.SetTextAndPrice(product.ItemName, product.ItemPrice);
        textAndPrice.OnRemovedProduct += deleteCallback;
        
        _productList.Add(textAndPrice);
        
        OnProductListChanged?.Invoke();
    }

    public float RemoveSelectedItemAndGetPrice(int selectedGlobalProductIndex)
    {
        bool isFound = false;
        float productPrice = 0;
        
        for (int i = 0; i < _productList.Count; i++)
        {
            if (_productList[i].ProductIndex == selectedGlobalProductIndex)
            {
                productPrice = _productList[i].Product.ItemPrice;
                isFound = true;
                
                Destroy(_productList[i].gameObject);
                _productList.RemoveAt(i);
                
                OnProductListChanged?.Invoke();
            }
        }

        if (!isFound)
        {
            Debug.Log("Product not found");
            return 0;
        }
        else
        {
            return productPrice;
        }
    }

    private void ResetRegisteredProducts()
    {
        foreach (RectTransform child in _productListLayout.transform)
        {
            Destroy(child.gameObject);
        }
        
        OnProductListChanged?.Invoke();
    }
}
