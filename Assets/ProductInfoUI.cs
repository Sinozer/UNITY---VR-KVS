using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ProductInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _productNameText;
    [SerializeField] private TextMeshProUGUI _productPriceText;

    public void SetProductInfo(string productName, float productPrice)
    {
        _productNameText.text = productName;
        _productPriceText.text = "$ " + productPrice;
    }
}
