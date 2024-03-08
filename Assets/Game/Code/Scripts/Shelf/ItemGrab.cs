using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private int _maxItemNumber;

    [SerializeField] private ItemSo _itemInfo; 
    
    [Header("Debug")]
    [SerializeField, ReadOnly] private int _currentItemNumber;
    
    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemValueText;
    [SerializeField] private TextMeshProUGUI _itemPrice;

    private void Awake()
    {
        _itemValueText.text = _maxItemNumber.ToString();
        _currentItemNumber = _maxItemNumber;
        _itemNameText.text = _itemInfo.ItemName;
        _itemPrice.text = _itemInfo.ItemPrice.ToString();
        //TODO Get the item name from the ScriptableObject && price 
    }

    public void PickupItem()
    {
        if (_currentItemNumber <= 0) return; 
        _currentItemNumber--;
        _itemValueText.text = _currentItemNumber.ToString();
        
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
        
        Instantiate(_itemInfo.Prefab, spawnPosition, Quaternion.identity);
    }
}
