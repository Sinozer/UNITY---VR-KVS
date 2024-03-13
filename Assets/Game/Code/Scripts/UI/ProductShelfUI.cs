using System;
using TMPro;
using UnityEngine;

namespace Game.Code.Scripts.UI
{
    public class ProductShelfUI : MonoBehaviour
    {
        public String NameText
        {
            get => _itemNameText.text;
            set => _itemNameText.text = value;
        }
        
        public String ValueText
        {
            get => _itemValueText.text;
            set => _itemValueText.text = value;
        }
        
        public String PriceText
        {
            get => _itemPrice.text;
            set => _itemPrice.text = value;
        }
        
        [Header("UI Texts")]
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemValueText;
        [SerializeField] private TextMeshProUGUI _itemPrice;
    }
}