using System;
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
    

    public void ShowClientInfo(ClientSo client)
    {
        _clientName.text = client.ClientName;
        UpdateHumor(client.BaseHumor);
        FormatAndSetTimerText(client.TimerInSeconds);
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

    public void FormatAndSetTimerText(int timer)
    {
        _sb.Clear();

        int minutes = Mathf.FloorToInt(timer / 60.0f);
        int seconds = Mathf.FloorToInt(timer % 60.0f);

        _sb.Append(minutes.ToString("00"));
        _sb.Append(":");
        _sb.Append(seconds.ToString("00"));

        _timerText.text = _sb.ToString();
    }
}
