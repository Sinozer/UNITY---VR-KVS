using System.Globalization;
using FiniteStateMachine;
using TMPro;
using UnityEngine;

public class PaymentView : BaseState
{
    public float EnteredPrice => _numericalPad.GetEnteredPrice();
    
    [SerializeField] private NumericalPad _numericalPad;
    [SerializeField] private TextMeshProUGUI _scannedPriceUI;
    
    private float _scannedPrice;


    public override void Enter(StateMachine manager, params object[] args)
    {
        base.Enter(manager, args);
        
        gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        
        gameObject.SetActive(false);
    }

    public void InitNumericalView(float scannedPrice)
    {
        _scannedPriceUI.text = scannedPrice.ToString("00.00");
        _numericalPad.ResetNumericalPad();
    }
}
