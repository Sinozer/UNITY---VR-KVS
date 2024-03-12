using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientSatisfactionUI : MonoBehaviour
{
    [Header("Client Info")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _clientName;
    [SerializeField] private Image _satisfactionImage;
    
    [Header("Satisfaction faces")]
    [SerializeField] private Sprite _happyFace;
    [SerializeField] private Sprite _indifferentFace;
    [SerializeField] private Sprite _angryFace;


    private readonly StringBuilder _sb = new();
    private int _clientTimer;
    private bool _isDone;

    private Coroutine _clientCoroutine;

    

    public void ShowClientInfo(ClientSo client)
    {
        _clientName.text = client.ClientName;
        SetClientTimer(client.TimerInSeconds);
        UpdateHumor(client.BaseHumor);
    }

    private void UpdateHumor(ClientHumor humor)
    {
        _satisfactionImage.sprite = humor switch
        {
            ClientHumor.HAPPY => _happyFace,
            ClientHumor.INDIFFERENT => _indifferentFace,
            ClientHumor.ANGRY => _angryFace,
            _ => throw new ArgumentOutOfRangeException(nameof(humor), humor, null)
        };
    }

    private void SetClientTimer(int clientTimer)
    {
        _clientTimer = clientTimer;
        FormatAndSetTimerText();
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
            
            _clientTimer--;
            
            FormatAndSetTimerText();
        }
    }

    private void FormatAndSetTimerText()
    {
        _sb.Clear();

        int minutes = Mathf.FloorToInt(_clientTimer / 60.0f);
        int seconds = Mathf.FloorToInt(_clientTimer % 60.0f);

        _sb.Append(minutes.ToString("00"));
        _sb.Append(":");
        _sb.Append(seconds.ToString("00"));

        _timerText.text = _sb.ToString();
    }
}
