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

    
    [ShowInInspector, ReadOnly] private ClientSo _client;
    
    [Header("Items to buy")] [OdinSerialize]
    private Dictionary<ItemSo, int> _shoppingList;
    
    
    [Header("Item forgotten")] [OdinSerialize, ReadOnly]
    private ItemSo _forgottenItem;
    
    // [SerializeField, Range(0, 1)]
    private float _forgottenItemChance = 1f;

    [ShowInInspector, ReadOnly] private ClientHumor _clientHumor;
    [ShowInInspector, ReadOnly] private float _clientSatisfaction;
    [ShowInInspector, ReadOnly] private int _clientTimer;
    private float _clientSatisfactionStep;
    private bool _isDone;
    private Coroutine _clientTimerCoroutine;
    private List<Furniture> _spawnedItems;

    
    public event Action<int> OnTimerUpdated;


    public void Initialize(ClientSo client)
    {
        _client = client;
        
        _clientTimer = _client.ClientConfig.TimerInSeconds;
        _clientSatisfaction = _client.ClientConfig.BaseSatisfaction;
        _clientHumor = _client.ClientConfig.BaseHumor;
        _clientSatisfactionStep = (_client.ClientConfig.BaseSatisfaction / _client.ClientConfig.TimerInSeconds) * 0.5f;
        
        _isDone = false;
    }
    
    private void Start()
    {
        _shoppingList = new Dictionary<ItemSo, int>();
        _spawnedItems = new List<Furniture>();

        for (int i = 0; i < _client.ClientConfig.MaxDifferentItems; i++)
        {
            ItemSo randomItem = GameManager.ItemRegistry.RandomItem;
            if (_shoppingList.ContainsKey(randomItem)) return;

            _shoppingList.Add(randomItem, Random.Range(1, _client.ClientConfig.MaxNumberOfSameItems));
        }
        
        if (Random.value < _forgottenItemChance)
            IsMissingItem();
    }
    
    public IEnumerator SpawnItems(BoxCollider spawnArea)
    {
        _clientTimerCoroutine = StartCoroutine(ClientTimerCoroutine());
        
        foreach (KeyValuePair<ItemSo, int> item in _shoppingList)
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
        _clientSatisfaction -= _clientSatisfactionStep;
        UpdateClientHumor();
    }

    private void UpdateClientHumor()
    {
        _clientHumor = _client.ClientConfig.DetermineHumor(_clientSatisfaction);
        UIManager.Instance.UpdateClientHumor(_clientHumor);
    }

    public void PrepareToLeave(float totalScannedPrice)
    {
        _isDone = true;
        
        float supposedTotal = _spawnedItems.Sum(item => item.ProductSo.ItemPrice);
        float satisfactionGainedFromPrice = totalScannedPrice / supposedTotal;

        _clientSatisfaction *= satisfactionGainedFromPrice;

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
    
    private void IsMissingItem()
    {
        int randomIndex = Random.Range(0, _shoppingList.Count - 1);
        
        _forgottenItem = _shoppingList.Keys.ElementAt(randomIndex);
        _shoppingList.Remove(_forgottenItem);
        
        UIManager.Instance.ProductUI.Item = _forgottenItem;
    }
}