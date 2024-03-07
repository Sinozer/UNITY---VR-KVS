using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCashout : MonoBehaviour
{
    public float totalScannedPrice => _totalScannedPrice;
    
    [SerializeField] private TextAndPriceUI _lastProductDisplayUI;
    [SerializeField] private ProductListUI _productListUI;
    [SerializeField] private TextAndPriceUI _totalTextUI;
    
    [SerializeField] private List<ItemSo> _items;

    private float _totalScannedPrice;
    
    public event Action OnItemScanned;


    private void OnEnable()
    {
        _productListUI.OnProductListChanged += () => _totalTextUI.SetPrice(_totalScannedPrice);
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
        _productListUI.AddItemInfoToList(product);
        
        OnItemScanned?.Invoke();
    }

    public void ResetCashout()
    {
        _totalScannedPrice = 0;
        
        _productListUI.ResetRegisteredProducts();
        _totalTextUI.ResetPrice();
        _lastProductDisplayUI.ResetAll();
    }

    public void ShowTotal()
    {
        _lastProductDisplayUI.SetTextAndPrice("Total:", _totalScannedPrice);
    }
}
