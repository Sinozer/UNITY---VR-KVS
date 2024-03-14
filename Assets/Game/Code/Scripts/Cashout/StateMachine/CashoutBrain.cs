using System;
using System.Collections;
using FiniteStateMachine;
using Game.Code.Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;

public class CashoutBrain : StateMachine
{
    public float TotalScannedPrice { get; private set; }
    
    [SerializeField] private WaitingState _waitingState;
    [SerializeField] private SpawningItemsState _spawningItemsState;
    [SerializeField] private PaymentState _paymentState;
    
    [Header("Clients")] 
    [SerializeField] private ClientHolder _clientHolder;

    [Header("Waypoints")] 
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _itemDeliveryPoint;
    [SerializeField] private Transform _paymentPoint;
    [SerializeField] private Transform _leavePoint;
    
    [Header("Door")]
    [SerializeField] private GameObject _door;
    [SerializeField] private AnimationCurve _doorCurve;
    [SerializeField, Range(-180, 180)] private float _doorAngle;
    [SerializeField, Range(0, 10)] private float _delayBeforeDoorClose;
    
    [Header("Furniture")]
    [SerializeField] private FurnitureScanner _furnitureScanner;
    [SerializeField] private BoxCollider _furnitureSpawnArea;
    
    [Header("UI")]
    [SerializeField] private CashoutDisplayUI _cashoutDisplayUI;
    [SerializeField] private TextAndPriceUI _lastProductDisplayUI;
    [SerializeField] private TextAndPriceUI _totalTextUI;
    
    [Header("Events")]
    [SerializeField] private ItemEventSO _onProductScanned;
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _confirmPaymentSong;

    private int _globalProductIndex;

    public event Action<float> OnTotalPriceRegistered;
    
    
    private void OnEnable()
    {
        // _furnitureScanner.OnItemScanned += RegisterItemToCashout;
        _onProductScanned.OnEvent += RegisterItemToCashout;
    }
    
    private void OnDisable()
    {
        // _furnitureScanner.OnItemScanned -= RegisterItemToCashout;
        _onProductScanned.OnEvent -= RegisterItemToCashout;
    }
    
    private void Start()
    {
        ResetCashout();
        
        _clientHolder.Initialize(_spawnPoint);
        _clientHolder.StartSpawning();
        
        GoBackToWaitingState();
    }

    private void RegisterItemToCashout(ItemSo product)
    {
        _globalProductIndex++;
        TotalScannedPrice += product.ItemPrice;
        
        _lastProductDisplayUI.SetTextAndPrice(product.ItemName, product.ItemPrice);
        _cashoutDisplayUI.AddItemInfoToList(product, _globalProductIndex, RemoveSelectedProduct);

        UpdateTotalPriceText();
    }
    
    private void RemoveSelectedProduct(int index)
    {
        float productPrice = _cashoutDisplayUI.RemoveSelectedItemAndGetPrice(index);
        TotalScannedPrice -= productPrice;
        
        UpdateTotalPriceText();
    }

    private void ResetCashout()
    {
        _globalProductIndex = 0;
        TotalScannedPrice = 0;
        
        _cashoutDisplayUI.SetProductListView();
        
        _totalTextUI.ResetPrice();
        _lastProductDisplayUI.ResetAll();

        UpdateTotalPriceText();
    }

    private void UpdateTotalPriceText()
    {
        _totalTextUI.SetPrice(Mathf.Round(TotalScannedPrice * 100f) / 100f);
    }

    [Button]
    public void ShowPaymentView()
    {
        _lastProductDisplayUI.SetTextAndPrice("Total:", TotalScannedPrice);
        _cashoutDisplayUI.SetPaymentView(TotalScannedPrice);
    }

    public void ConfirmPaymentPrice()
    {
        if (_clientHolder.CurrentClient)
        {
            float enteredPrice = _cashoutDisplayUI.EnteredPrice;
            OnTotalPriceRegistered?.Invoke(enteredPrice);
            _audioSource.PlayOneShot(_confirmPaymentSong);
            
            GameManager.Instance.UpdateQuota(enteredPrice, _clientHolder.CurrentClient.ClientSatisfaction);
            
            ResetCashout();
        }
    }
    
    private void GoBackToWaitingState()
    {
        UIManager.Instance.UpdateClientTracker(_clientHolder.CurrentClient.client);
        
        ChangeState(_waitingState, _clientHolder.CurrentClient, _itemDeliveryPoint);
        _waitingState.OnClientArrivedAtDeliveryPoint += OnClientArrivedAtDeliveryPoint;
    }
    
    private void OnClientArrivedAtDeliveryPoint()
    {
        _clientHolder.CurrentClient.OnTimerUpdated += UIManager.Instance.UpdateClientTimer;
        ChangeState(_spawningItemsState, _clientHolder.CurrentClient, _furnitureSpawnArea);
        _spawningItemsState.OnClientItemsSpawned += OnClientItemsSpawned;
    }
    
    private void OnClientItemsSpawned()
    {
        ChangeState(_paymentState, _clientHolder.CurrentClient, _paymentPoint, _leavePoint);
        _paymentState.OnClientFinished += OnClientFinished;
    }
    
    private void OnClientFinished()
    {
        StartCoroutine(DoorCoroutine());
        
        GoBackToWaitingState();

        return;
        
        IEnumerator DoorCoroutine()
        {
            yield return StartCoroutine(SetDoorRotation(true));
            
            yield return new WaitForSeconds(_delayBeforeDoorClose);
            
            yield return StartCoroutine(SetDoorRotation(false));
            
            _clientHolder.OnCurrentClientFinished();
        }
    }
    
    private IEnumerator SetDoorRotation(bool shouldOpen)
    {
        float time = 0.0f;
        float duration = _doorCurve.keys[_doorCurve.length - 1].time;
        float angleA = shouldOpen ? 0.0f : _doorAngle;
        float angleB = shouldOpen ? _doorAngle : 0.0f;

        while (time < duration)
        {
            float rotationAngle = Mathf.Lerp(angleA, angleB, _doorCurve.Evaluate(time));
            _door.transform.rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);

            time += Time.deltaTime;
            yield return null;
        }
    }
    
    
}