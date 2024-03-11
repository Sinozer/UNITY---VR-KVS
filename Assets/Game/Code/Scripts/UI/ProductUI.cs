using Sirenix.OdinInspector;
using UnityEngine;

public class ProductUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _productImageHolder;
    [SerializeField] private TMPro.TextMeshProUGUI _productNameHolder;

    [SerializeField] private ItemSo _item;
    
    public ItemSo Item
    {
        get => _item;
        set
        {
            _item = value;
            // Get the image from the prefab model
            _productImageHolder.sprite = _item.ItemImage;
            _productNameHolder.text = _item.ItemName;
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
}
