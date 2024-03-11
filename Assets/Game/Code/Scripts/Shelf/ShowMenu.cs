using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ShowMenu : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool _isTrigger;
    
    [Header("Events")]
    [SerializeField] private UnityEvent _showOn;
    [SerializeField] private UnityEvent _showOff;

    public void MenuManager()
    {
        _isTrigger = !_isTrigger;

        if (_isTrigger)
        {
            _showOn.Invoke();
        }
        else
        {
            _showOff.Invoke();
        }
    }
}
