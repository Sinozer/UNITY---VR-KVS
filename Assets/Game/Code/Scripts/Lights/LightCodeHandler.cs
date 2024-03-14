using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightCodeHandler : MonoBehaviour
{
	[Header("Lights")]
	[SerializeField, Range(10, 600)] private float _minTimerBeforeSwitch = 60;
	[SerializeField, Range(10, 600)] private float _maxTimerBeforeSwitch = 120;
	
	[Header("Code")]
	[SerializeField] private NumericalPad _numericalPad;
	[SerializeField] private TextMeshProUGUI _displayedCode;
	[SerializeField, Range(2,8)] private int _codeLength = 4;
	[SerializeField, ReadOnly] private string _code;
	
	public void OnConfirmPressed()
	{
		if (_numericalPad.GetEnteredPriceAsString() == _code)
		{
			TurnLightsOn();
		}
	}
	
	private SwitchLights _switchLights;

	private void Start()
	{
		_switchLights = GetComponent<SwitchLights>();
		StartLightsSwitchTimer();
	}
	
	private void ResetCode()
	{
		_code = "00";
		_displayedCode.text = _code;
		_numericalPad.ResetNumericalPad();
	}
	
	private void GenerateCode()
	{
		_code = "";
		for (int i = 0; i < _codeLength; i++)
		{
			_code += Random.Range(0, 10).ToString();
		}
		_displayedCode.text = _code;
	}
	
	private void TurnLightsOn()
	{
		_switchLights.TurnLightsOn();
		ResetCode();
		StartLightsSwitchTimer();
	}

	private void LightsOff()
	{
		_switchLights.TurnLightsOff();
		GenerateCode();
	}
    
	private void StartLightsSwitchTimer()
	{
		float timer = Random.Range(_minTimerBeforeSwitch, _maxTimerBeforeSwitch);
		Invoke(nameof(LightsOff), timer);
	}
}