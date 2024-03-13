using System;
using Game.Code.Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProductUI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private ItemEventSO _onProductScanned;
    
    [SerializeField] private UnityEngine.UI.Image _productImageHolder;
    [SerializeField] private TMPro.TextMeshProUGUI _productNameHolder;

    [SerializeField] private ItemSo _item;
    
    public ItemSo Item
    {
        get => _item;
        set
        {
            if (value == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _item = value;
            // Get the image from the prefab model
            _productImageHolder.sprite = _item.ItemImage;
            _productNameHolder.text = _item.ItemName;
            
            gameObject.SetActive(true);
        }
    }
    
    #if UNITY_EDITOR
    [Button]
    private void Refresh()
    {
        if (_item == null)
            return;
        
        _productImageHolder.sprite = _item.ItemImage;
        _productNameHolder.text = _item.ItemName;
    }
    #endif

    private void OnEnable()
    {
        _onProductScanned.OnEvent += OnProductScanned;
    }
    
    private void OnDisable()
    {
        _onProductScanned.OnEvent -= OnProductScanned;
    }
    
    private void OnProductScanned(ItemSo so)
    {
        if (so == _item)
        {
            _item = null;
            gameObject.SetActive(false);
            _productImageHolder.sprite = null;
            _productNameHolder.text = "HASSOUL TUTTO BENE";
        }
    }
}
