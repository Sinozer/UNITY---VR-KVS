using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumericalPad : MonoBehaviour
{
    [SerializeField] private Button _confirmButton;
    [SerializeField] private TextMeshProUGUI _enteredPriceUI;
    
    public UnityEvent onConfirmPressed;
    
    private readonly StringBuilder _stringBuilder = new();
    private bool _isDecimalAdded;
    

    private void OnEnable()
    {
        _confirmButton.onClick.AddListener(ConfirmEnteredPrice);
    }

    private void OnDisable()
    {
        _confirmButton.onClick.RemoveListener(ConfirmEnteredPrice);
    }

    public void ResetNumericalPad()
    {
        _stringBuilder.Clear();
        _isDecimalAdded = false;
        RefreshTextUI();
    }

    public void AddNumber(string numberToAdd)
    {
        if (_stringBuilder.Length == 1 && numberToAdd == "0") return;
        
        _stringBuilder.Append(numberToAdd);
        RefreshTextUI();
    }

    public void DeleteLastChar()
    {
        if (_stringBuilder.Length == 0) return;
        
        char lastChar = _stringBuilder[^1];
        if (lastChar == '.') _isDecimalAdded = false;
        
        _stringBuilder.Remove(_stringBuilder.Length - 1, 1);
        RefreshTextUI();
    }

    private void RefreshTextUI()
    {
        _enteredPriceUI.text = _stringBuilder.ToString();
    }

    public void SetDecimalPoint()
    {
        if (_isDecimalAdded) return;
        
        _stringBuilder.Append(",");
        _isDecimalAdded = true;
        RefreshTextUI();
    }

    private void ConfirmEnteredPrice()
    {
        onConfirmPressed?.Invoke();
    }

    public float GetEnteredPrice()
    {
        if (float.TryParse(_stringBuilder.ToString(), out float result))
        {
            return result;
        }
        else
        {
            return 0;
        }
    }
}
