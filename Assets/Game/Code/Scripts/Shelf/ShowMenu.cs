using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ShowMenu : MonoBehaviour
{
    [SerializeField] private bool _toggle;
    [SerializeField] private UnityEvent _showOn;
    [SerializeField] private UnityEvent _showOff;
    
    public void MenuManager()
    {
        _toggle = !_toggle;
        
        if (_toggle)
        {
            _showOn.Invoke();
        }
        else
        {
            _showOff.Invoke();
        }
    }
    
}
