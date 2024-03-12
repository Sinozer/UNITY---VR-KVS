using UnityEngine;


public class QuotaUI : MonoBehaviour
{
    [SerializeField] private SliderQuota _moneyQuota;
    [SerializeField] private SliderQuota _clientSatisfactionQuota;


    public void UpdateMoneyQuota(float currentQuota, float maxQuota)
    {
        _moneyQuota.UpdateSliderQuota(currentQuota, maxQuota);
    }

    public void UpdateClientQuota(float currentQuota, float maxQuota)
    {
        _clientSatisfactionQuota.UpdateSliderQuota(currentQuota, maxQuota);
    }
}
