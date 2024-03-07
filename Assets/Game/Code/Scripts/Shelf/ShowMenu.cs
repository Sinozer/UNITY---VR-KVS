using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ShowMenu : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private int _maxItemNumber;
    
    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemValueText;
    
    [Header("Debug")]
    [SerializeField] private bool _isTrigger;
    [SerializeField] private int _currentItemNumber;
    
    [Header("Events")]
    [SerializeField] private UnityEvent _showOn;
    [SerializeField] private UnityEvent _showOff;

    private void Awake()
    {
        _itemValueText.text = _maxItemNumber.ToString();
        _currentItemNumber = _maxItemNumber;
        //TODO Get the item name from the ScriptableObject
    }

    public void MenuManager()
    {
        _isTrigger = !_isTrigger;

        if (_isTrigger)
        {
            _showOn.Invoke();
            if(_currentItemNumber > 0) PickupItem();
        }
        else
        {
            _showOff.Invoke();
        }
    }

    public void PickupItem()
    {
        _currentItemNumber--;
        _itemValueText.text = _currentItemNumber.ToString();
    }
    

}
