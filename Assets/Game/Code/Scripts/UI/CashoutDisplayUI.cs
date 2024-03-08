using System;
using FiniteStateMachine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CashoutDisplayUI : StateMachine
{
    public float EnteredPrice => _paymentView.EnteredPrice;

    [SerializeField] private VerticalLayoutGroup _productListLayout;
    [SerializeField, AssetsOnly] private TextAndPriceUI _textAndPricePrefab;
    [SerializeField] private ProductListView _productListView;
    [SerializeField] private PaymentView _paymentView;
    [SerializeField] private TextMeshProUGUI _headerText;
    
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
    
    public void AddItemInfoToList(ItemSo product)
    {
        TextAndPriceUI textAndPrice = Instantiate(_textAndPricePrefab.gameObject, _productListLayout.transform)
            .GetComponent<TextAndPriceUI>();
        textAndPrice.SetTextAndPrice(product.ItemName, product.ItemPrice);
        
        OnProductListChanged?.Invoke();
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
