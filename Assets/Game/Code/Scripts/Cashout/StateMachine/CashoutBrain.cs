using System;
using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine;
using UnityEngine;

public class CashoutBrain : StateMachine
{
    public float TotalScannedPrice { get; private set; }
    
    public event Action OnItemScanned;

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
    [SerializeField] private BoxCollider _furnitureSpawnArea;
    
    [Header("UI")]
    [SerializeField] private TextAndPriceUI _lastProductDisplayUI;
    [SerializeField] private ProductListUI _productListUI;
    [SerializeField] private TextAndPriceUI _totalTextUI;
    
    [SerializeField] private List<ItemSo> _items;

    public void RegisterItemToCashout(ItemSo product)
    {
        TotalScannedPrice += product.ItemPrice;
        
        _lastProductDisplayUI.SetTextAndPrice(product.ItemName, product.ItemPrice);
        _productListUI.AddItemInfoToList(product);
        
        OnItemScanned?.Invoke();
    }

    public void ResetCashout()
    {
        TotalScannedPrice = 0;
        
        _productListUI.ResetRegisteredProducts();
        _totalTextUI.ResetPrice();
        _lastProductDisplayUI.ResetAll();
    }

    public void ShowTotal()
    {
        _lastProductDisplayUI.SetTextAndPrice("Total:", TotalScannedPrice);
    }
    
    private void OnEnable()
    {
        _productListUI.OnProductListChanged += () => _totalTextUI.SetPrice(TotalScannedPrice);
    }
    
    private void Start()
    {
        ResetCashout();
        
        _clientHolder.Initialize(_spawnPoint);
        _clientHolder.StartSpawning();
        
        GoBackToWaitingState();
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