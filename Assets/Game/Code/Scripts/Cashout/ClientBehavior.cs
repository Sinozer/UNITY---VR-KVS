using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClientBehavior : SerializedMonoBehaviour
{
    public Dictionary<ItemSo, int> ShoppingList => _shoppingList;
    
    public float DelayBetweenItems => _delayBetweenItems;
    
    [Header("Items to buy")] 
    [OdinSerialize] private Dictionary<ItemSo, int> _shoppingList;

    [SerializeField, Range(1, 15)] private int _maxDifferentItems;
    [SerializeField, Range(1, 100)] private int _maxItemsPerCategory;

    [Header("Client behavior")] 
    [SerializeField, Range(0, 10)] private float _delayBetweenItems;

    private void Start()
    {
        _shoppingList = new Dictionary<ItemSo, int>();

        for (int i = 0; i < _maxDifferentItems; i++)
        {
            ItemSo randomItem = ItemRegistry.Instance.RandomItem;
            if (_shoppingList.ContainsKey(randomItem)) return;

            _shoppingList.Add(randomItem, Random.Range(1, _maxItemsPerCategory));
        }
    }
}