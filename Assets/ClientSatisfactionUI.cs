using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientSatisfactionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _clientName;
    [SerializeField] private Image _satisfactionImage;

    private readonly StringBuilder _sb = new();
    private int _clientTimer;
    private bool _isDone;

    private Coroutine _clientCoroutine;

    

    public void ShowClient()
    {
        
    }
    
    public void StartTimer()
    {
        _clientCoroutine = StartCoroutine(StartClientTimer());
    }

    public void FinishedClient()
    {
        _isDone = true;
        
        if (_clientCoroutine != null)
        {
            StopCoroutine(_clientCoroutine);
            _clientCoroutine = null;
        }
    }

    private IEnumerator StartClientTimer()
    {
        while (!_isDone)
        {
            yield return new WaitForSeconds(1);

            _sb.Clear();
            
            _clientTimer--;
            
            int minutes = Mathf.FloorToInt(_clientTimer / 60.0f);
            int seconds = Mathf.FloorToInt(_clientTimer % 60.0f);

            _sb.Append(minutes.ToString("00"));
            _sb.Append(":");
            _sb.Append(seconds.ToString("00"));

            _timerText.text = _sb.ToString();
        }
    }
}
