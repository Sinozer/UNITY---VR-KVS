using System;
using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine;
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
    [SerializeField] private GameObject _door;
    
    [Header("Furniture")]
    [SerializeField] private FurnitureScanner _furnitureScanner;
    [SerializeField] private BoxCollider _furnitureSpawnArea;
    
    [Header("UI")]
    [SerializeField] private CashoutDisplayUI _cashoutDisplayUI;
    [SerializeField] private TextAndPriceUI _lastProductDisplayUI;
    [SerializeField] private TextAndPriceUI _totalTextUI;
    
    public event Action OnItemRegistered;
    
    
    private void OnEnable()
    {
        _cashoutDisplayUI.OnProductListChanged += () => _totalTextUI.SetPrice(TotalScannedPrice);
        _furnitureScanner.OnItemScanned += RegisterItemToCashout;
    }
    
    private void OnDisable()
    {
        _furnitureScanner.OnItemScanned -= RegisterItemToCashout;
    }
    
    private void Start()
    {
        ResetCashout();
        
        _clientHolder.Initialize(_spawnPoint);
        _clientHolder.StartSpawning();
        
        GoBackToWaitingState();
    }
    
    public void RegisterItemToCashout(ItemSo product)
    {
        TotalScannedPrice += product.ItemPrice;
        
        _lastProductDisplayUI.SetTextAndPrice(product.ItemName, product.ItemPrice);
        _cashoutDisplayUI.AddItemInfoToList(product);
        
        OnItemRegistered?.Invoke();
    }

    public void ResetCashout()
    {
        TotalScannedPrice = 0;
        
        _cashoutDisplayUI.SetProductListView();
        
        _totalTextUI.ResetPrice();
        _lastProductDisplayUI.ResetAll();
    }

    [Button]
    public void ShowPaymentView()
    {
        _lastProductDisplayUI.SetTextAndPrice("Total:", TotalScannedPrice);
        _cashoutDisplayUI.SetPaymentView(TotalScannedPrice);
    }

    public void ConfirmPaymentPrice()
    {
        float enteredPrice = _cashoutDisplayUI.EnteredPrice;
        ResetCashout();
    }
    
    private void GoBackToWaitingState()
    {
        ChangeState(_waitingState, _clientHolder.CurrentClient, _itemDeliveryPoint);
        _waitingState.OnClientArrivedAtDeliveryPoint += OnClientArrivedAtDeliveryPoint;
    }
    
    private void OnClientArrivedAtDeliveryPoint()
    {
        ChangeState(_spawningItemsState, _clientHolder.CurrentClient, _furnitureSpawnArea);
        _spawningItemsState.OnClientItemsSpawned += OnClientItemsSpawned;
    }
    
    private void OnClientItemsSpawned()
    {
        ChangeState(_paymentState, _clientHolder.CurrentClient, _paymentPoint);
        _paymentState.OnClientFinished += OnClientFinished;
    }
    
    private void OnClientFinished()
    {
        _clientHolder.OnCurrentClientFinished();
        
        GoBackToWaitingState();
        
        _door.SetActive(false);
    }
}