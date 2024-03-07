using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class PlayerCashout : MonoBehaviour
{
    [SerializeField] private ProductDisplayUI _productDisplayUI;
    [SerializeField] private ProductListUI _productListUI;
    [SerializeField] private List<ItemSo> _item;
    
    public event Action OnItemScanned;

    
    private void Awake()
    {
        StartCoroutine(AddItemCoroutine());

        IEnumerator AddItemCoroutine()
        {
            foreach (var item in _item)
            {
                AddItemToCashout(item);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void AddItemToCashout(ItemSo product)
    {
        _productDisplayUI.UpdateLastProductInfo(product);
        _productListUI.AddItemInfoToList(product);
        OnItemScanned?.Invoke();
    }
}
