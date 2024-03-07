using System;
using TMPro;
using UnityEngine;

public class TextAndPriceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _contentTitleText;
    [SerializeField] private TextMeshProUGUI _productPriceText;

    
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
        _productPriceText.text = "$ " + price;
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
