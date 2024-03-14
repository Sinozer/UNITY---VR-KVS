using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private ProductUI _productUI;
    [SerializeField] private ClientSatisfactionUI _clientSatisfactionUI;
    [SerializeField] private QuotaUI _quotaUI;
    

    private void Awake()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.OnMoneyQuotaUpdated += _quotaUI.UpdateMoneyQuota;
            GameManager.Instance.OnClientSatisfactionUpdated += _quotaUI.UpdateClientQuota;
        }

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
    }

    private void Start()
    {
        HideProductTracker();
        HideClientTracker();
    }

    public void UpdateClientTracker(ClientSo client)
    {
        if (!_clientSatisfactionUI.gameObject.activeSelf)
            _clientSatisfactionUI.gameObject.SetActive(true);
        
        _clientSatisfactionUI.ShowClientInfo(client);
    }

    public void UpdateProductTracker(ItemSo product)
    {
        if (!_productUI.gameObject.activeSelf)
            _productUI.gameObject.SetActive(true);
        
        _productUI.Item = product;
    }

    public void UpdateClientTimer(int timer)
    {
        _clientSatisfactionUI.FormatAndSetTimerText(timer);
    }

    public void UpdateClientHumor(ClientHumor clientHumor)
    {
        _clientSatisfactionUI.UpdateHumor(clientHumor);
    }

    public void HideProductTracker()
    {
        _productUI.gameObject.SetActive(false);
    }

    public void HideClientTracker()
    {
        _clientSatisfactionUI.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.OnMoneyQuotaUpdated -= _quotaUI.UpdateMoneyQuota;
            GameManager.Instance.OnClientSatisfactionUpdated -= _quotaUI.UpdateClientQuota;
        }
    }
}
