using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerCashout : MonoBehaviour
{
    public float TotalScannedPrice => _totalScannedPrice;
    
    [SerializeField] private CashoutDisplayUI _cashoutDisplayUI;
    [SerializeField] private TextAndPriceUI _lastProductDisplayUI;
    [SerializeField] private TextAndPriceUI _totalTextUI;
    [SerializeField] private List<ItemSo> _items;

    
    private float _totalScannedPrice;
    
    public event Action OnItemScanned;


    private void OnEnable()
    {
        _cashoutDisplayUI.OnProductListChanged += () => _totalTextUI.SetPrice(_totalScannedPrice);
    }


    private void Start()
    {
        ResetCashout();
        
        StartCoroutine(AddItemCoroutine());

        IEnumerator AddItemCoroutine()
        {
            yield return new WaitForSeconds(5f);
            
            foreach (var item in _items)
            {
                RegisterItemToCashout(item);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void RegisterItemToCashout(ItemSo product)
    {
        _totalScannedPrice += product.ItemPrice;
        
        _lastProductDisplayUI.SetTextAndPrice(product.ItemName, product.ItemPrice);
        _cashoutDisplayUI.AddItemInfoToList(product);
        
        OnItemScanned?.Invoke();
    }

    public void ResetCashout()
    {
        _totalScannedPrice = 0;
        
        _cashoutDisplayUI.SetProductListView();
        
        _totalTextUI.ResetPrice();
        _lastProductDisplayUI.ResetAll();
    }
    
    [Button]
    public void ShowPaymentView()
    {
        _lastProductDisplayUI.SetTextAndPrice("Total:", _totalScannedPrice);
        _cashoutDisplayUI.SetPaymentView(_totalScannedPrice);
    }

    public void ConfirmPaymentPrice()
    {
        float enteredPrice = _cashoutDisplayUI.EnteredPrice;
        ResetCashout();
    }
}
