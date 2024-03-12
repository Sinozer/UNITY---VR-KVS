using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderQuota : MonoBehaviour
{
    [SerializeField] private Slider _moneyQuota;
    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _quotaPercentage;
    
    [Header("Colors")] 
    [SerializeField] private Color _reachedColor;
    [SerializeField] private Color _unReachedColor;
    
    
    public void UpdateSliderQuota(float currentQuota, float quotaToReach)
    {
        float quota = currentQuota / quotaToReach;
        float clampedQuota = Mathf.Clamp(quota, 0, 1);

        _fillImage.color = Color.Lerp(_unReachedColor, _reachedColor, clampedQuota);

        _moneyQuota.value = clampedQuota;

        quota *= 100;
        _quotaPercentage.text = quota.ToString(CultureInfo.InvariantCulture);
    }
}