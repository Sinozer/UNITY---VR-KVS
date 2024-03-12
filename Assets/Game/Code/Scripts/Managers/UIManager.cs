using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
        
    [SerializeField] private ProductUI _productUI;
    [SerializeField] private ClientSatisfactionUI _clientSatisfactionUI;
    [SerializeField] private QuotaUI _quotaUI;
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        HideProductTracker();
        HideClientTracker();
    }

    public void UpdateClientTracker()
    {
        if (!_clientSatisfactionUI.gameObject.activeSelf)
            _clientSatisfactionUI.gameObject.SetActive(true);

        _clientSatisfactionUI.ShowClient();
    }

    public void StartClientTimer()
    {
        _clientSatisfactionUI.StartTimer();
    }

    public void UpdateProductTracker(ItemSo product)
    {
        if (!_productUI.gameObject.activeSelf)
            _productUI.gameObject.SetActive(true);
        
        _productUI.Item = product;
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
