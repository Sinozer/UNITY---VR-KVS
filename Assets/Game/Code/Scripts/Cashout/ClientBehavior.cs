using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClientBehavior : SerializedMonoBehaviour
{
    [OdinSerialize] private Dictionary<ItemSo, int> _shoppingList;
    [SerializeField, Range(1, 15)] private int _maxDifferentItems;
    [SerializeField, Range(1, 100)] private int _maxItemsPerCategory;
        
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