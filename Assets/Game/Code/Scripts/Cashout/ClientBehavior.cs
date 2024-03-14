using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Game.Code.Scripts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClientBehavior : SerializedMonoBehaviour
{
    public Dictionary<ItemSo, int> ShoppingList => _shoppingList;
    public float ClientSatisfaction => _clientSatisfaction;
    public ClientSo client => _client;
    public ClientHumor ClientHumor => _clientHumor;
    public ItemSo ForgottenItem => _forgottenItem;

    
    [ShowInInspector, ReadOnly] private ClientSo _client;
    
    [Header("Items to buy")] [OdinSerialize]
    private Dictionary<ItemSo, int> _shoppingList;
    
    
    [Header("Item forgotten")] [OdinSerialize, ReadOnly]
    private ItemSo _forgottenItem;
    
    [ShowInInspector, ReadOnly] private ClientHumor _clientHumor;
    [ShowInInspector, ReadOnly] private float _clientSatisfaction;
    [ShowInInspector, ReadOnly] private int _clientTimer;
    private int _timerThreshold;

    private int _totalNumberProducts;
    private int _totalScannedItem;
    private float _supposedTotal;
    private float _clientSatisfactionStep;
    private bool _isDone;
    private bool _isForgottenItemScanned = false;
    private Coroutine _clientTimerCoroutine;
    private List<Furniture> _spawnedItems;

    
    public event Action<int> OnTimerUpdated;


    public void Initialize(ClientSo client)
    {
        _client = client;
        
        _clientTimer = _client.ClientConfig.TimerInSeconds;
        _timerThreshold = _clientTimer / 2;
        _clientSatisfaction = _client.ClientConfig.BaseSatisfaction;
        _clientHumor = _client.ClientConfig.BaseHumor;
        _clientSatisfactionStep = (_client.ClientConfig.BaseSatisfaction / _timerThreshold) * 0.5f;
        
        _isDone = false;
    }
    
    private void Start()
    {
        _shoppingList = new Dictionary<ItemSo, int>();
        _spawnedItems = new List<Furniture>();

        for (int i = 0; i < _client.ClientConfig.MaxDifferentItems; i++)
        {
            ItemSo randomItem = GameManager.ItemRegistry.RandomItem;
            if (_shoppingList.ContainsKey(randomItem))
            {
                i--;
                continue;
            }
            
            int randomAmount = Random.Range(1, _client.ClientConfig.MaxNumberOfSameItems);
            _shoppingList.Add(randomItem, randomAmount);
            _supposedTotal += randomItem.ItemPrice * randomAmount;
            _totalNumberProducts += randomAmount;
        }
        
        if (Random.value < _client.ClientConfig.ForgottenItemChance)
            SetForgottenProduct();
    }
    
    public IEnumerator SpawnItems(BoxCollider spawnArea)
    {
        _clientTimerCoroutine = StartCoroutine(ClientTimerCoroutine());
        
        var copyList = _shoppingList.ToArray();
        
        foreach (KeyValuePair<ItemSo, int> item in copyList)
        {
            for (int i = 0; i < item.Value; i++)
            {
                Vector3 spawnPoint = spawnArea.RandomPointInBounds();
                Furniture furniture = Instantiate(item.Key.Prefab, spawnPoint, Quaternion.identity)
                    .GetComponent<Furniture>();
                furniture.Initialize(item.Key);
                _spawnedItems.Add(furniture);
                yield return new WaitForSeconds(_client.ClientConfig.SpawnDelayBetweenItems);
            }
        }

        yield return null;
    }
    
    private IEnumerator ClientTimerCoroutine()
    {
        OnTimerUpdated?.Invoke(_clientTimer);

        while (!_isDone)
        {
            yield return new WaitForSeconds(1);
            
            UpdateClient();
            
            OnTimerUpdated?.Invoke(_clientTimer);
        }
    }

    private void UpdateClient()
    {
        _clientTimer--;
        
        if (_clientTimer <= _timerThreshold)
        {
            _clientSatisfaction -= _clientSatisfactionStep;
            UpdateClientHumor();
        }
    }

    private void UpdateClientHumor()
    {
        _clientHumor = _client.ClientConfig.DetermineHumor(_clientSatisfaction);
        UIManager.Instance.UpdateClientHumor(_clientHumor);
    }

    public void PrepareToLeave(float totalScannedPrice)
    {
        _isDone = true;
        
        CalculateFinalClientSatisfaction(totalScannedPrice);

        foreach (Furniture item in _spawnedItems)
        {
            Destroy(item.gameObject);
        }

        if (_clientTimerCoroutine != null)
        {
            StopCoroutine(_clientTimerCoroutine);
            _clientTimerCoroutine = null;
        }
    }

    private void CalculateFinalClientSatisfaction(float totalScannedPrice)
    {
        // TODO: moved values to client config
        float satisfactionGainedFromPrice;
        if (totalScannedPrice > _supposedTotal * 1.1f)
        {
            satisfactionGainedFromPrice = 0.75f;
        }
        else if (totalScannedPrice < _supposedTotal * 1.1f)
        {
            satisfactionGainedFromPrice = 1.5f;
        }
        else if (Math.Abs(totalScannedPrice - _supposedTotal) < 0.1f)
        {
            satisfactionGainedFromPrice = 1.1f;
        }
        else
        {
            satisfactionGainedFromPrice = 1;
        }

        float satisfactionLostFromForgottenItem =
            _isForgottenItemScanned ? 1 : 1 - _client.ClientConfig.SatisfactionLossOnForgottenItem;
        
        int numberOfMissingItem = 0;
        foreach (var product in _shoppingList)
        {
            numberOfMissingItem += product.Value;
        }

        float satisfactionLossFromMissingItem = 1 - numberOfMissingItem * _client.ClientConfig.SatisfactionLossOnMissingItem;

        Debug.Log(satisfactionGainedFromPrice);
        Debug.Log(satisfactionLostFromForgottenItem);
        Debug.Log(satisfactionLossFromMissingItem);
        
        _clientSatisfaction *= satisfactionGainedFromPrice * satisfactionLossFromMissingItem *
                               satisfactionLostFromForgottenItem;
    }

    public void OnProductScanned(ItemSo product)
    {
        _totalScannedItem++;
        
        if (_shoppingList.ContainsKey(product))
        {
            if (_shoppingList[product] > 0)
            {
                _shoppingList[product]--;
            }
        }
        
        if (_forgottenItem == product)
        {
            _isForgottenItemScanned = true;
        }
    }
    
    public void OnProductRemoved(ItemSo product)
    {
        _totalScannedItem--;
        
        if (_shoppingList.ContainsKey(product))
        {
            _shoppingList[product]++;
        }
        
        if (_forgottenItem == product)
        {
            _isForgottenItemScanned = false;
        }
    }
    
    private void SetForgottenProduct()
    {
        int randomIndex = Random.Range(0, _shoppingList.Count - 1);
        
        _forgottenItem = _shoppingList.Keys.ElementAt(randomIndex);
        _shoppingList.Remove(_forgottenItem);
    }
}