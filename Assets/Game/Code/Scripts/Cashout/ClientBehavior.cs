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
    public float DelayBetweenItems => _delayBetweenItems;
    public ClientSo ClientConfig => _clientConfig;

    
    [ShowInInspector, ReadOnly] private ClientSo _clientConfig;
    
    [Header("Items to buy")] [OdinSerialize]
    private Dictionary<ItemSo, int> _shoppingList;

    [SerializeField, Range(1, 15)] private int _maxDifferentItems;
    [SerializeField, Range(1, 100)] private int _maxItemsPerCategory;
    
    [Header("Item forgotten")] [OdinSerialize, ReadOnly]
    private ItemSo _forgottenItem;
    
    // [SerializeField, Range(0, 1)]
    private float _forgottenItemChance = .3f;

    [Header("Client behavior")] [SerializeField, Range(0, 10)]
    private float _delayBetweenItems;

    private bool _isDone;
    private int _clientTimer;
    private Coroutine _clientTimerCoroutine;

    public event Action<int> OnTimerUpdated;
    

    public void Initialize(ClientSo client)
    {
        _clientConfig = client;
        _clientTimer = _clientConfig.TimerInSeconds;
        _isDone = false;
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
                yield return new WaitForSeconds(_delayBetweenItems);
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
            
            _clientTimer--;
            
            OnTimerUpdated?.Invoke(_clientTimer);
        }
    }

    public void PrepareToLeave(float totalScannedPrice)
    {
        _isDone = true;
        
        //TODO : Handle client satisfaction based on the totalScannedPrice
        float supposedTotal = _spawnedItems.Sum(item => item.ProductSo.ItemPrice);
        float satisfaction = totalScannedPrice / supposedTotal;

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

    private List<Furniture> _spawnedItems;

    private void Start()
    {
        _shoppingList = new Dictionary<ItemSo, int>();
        _spawnedItems = new List<Furniture>();

        for (int i = 0; i < _maxDifferentItems; i++)
        {
            ItemSo randomItem = GameManager.ItemRegistry.RandomItem;
            if (_shoppingList.ContainsKey(randomItem)) return;

            _shoppingList.Add(randomItem, Random.Range(1, _maxItemsPerCategory));
        }
        
        if (Random.value < _forgottenItemChance)
            _forgottenItem = GameManager.ItemRegistry.RandomItem;
    }
}