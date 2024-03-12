using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    
    [SerializeField] private ProductUI _productUI;
    [SerializeField] private ClientSatisfactionUI _clientSatisfactionUI;
    [SerializeField] private QuotaUI _quotaUI;
    

    private void OnEnable()
    {
        GameManager.Instance.OnMoneyQuotaUpdated += _quotaUI.UpdateMoneyQuota;
        GameManager.Instance.OnClientSatisfactionUpdated += _quotaUI.UpdateClientQuota;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnMoneyQuotaUpdated -= _quotaUI.UpdateMoneyQuota;
        GameManager.Instance.OnClientSatisfactionUpdated -= _quotaUI.UpdateClientQuota;
    }

    private void Awake()
    {
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

    public void HideProductTracker()
    {
        _productUI.gameObject.SetActive(false);
    }

    public void HideClientTracker()
    {
        _clientSatisfactionUI.gameObject.SetActive(false);
    }
}
