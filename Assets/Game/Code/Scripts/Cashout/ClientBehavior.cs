﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Code.Scripts;
using Game.Code.Scripts.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class ClientBehavior : SerializedMonoBehaviour
{
    public Dictionary<ItemSo, int> ShoppingList => _shoppingList;

    public float DelayBetweenItems => _delayBetweenItems;

    [Header("Items to buy")] [OdinSerialize]
    private Dictionary<ItemSo, int> _shoppingList;

    [SerializeField, Range(1, 15)] private int _maxDifferentItems;
    [SerializeField, Range(1, 100)] private int _maxItemsPerCategory;

    [Header("Client behavior")] [SerializeField, Range(0, 10)]
    private float _delayBetweenItems;

    public IEnumerator SpawnItems(BoxCollider spawnArea)
    {
        foreach (KeyValuePair<ItemSo, int> item in _shoppingList)
        {
            for (int i = 0; i < item.Value; i++)
            {
                Vector3 spawnPoint = spawnArea.RandomPointInBounds();
                FurnitureBehavior go = Instantiate(item.Key.Prefab, spawnPoint, Quaternion.identity)
                    .GetComponent<FurnitureBehavior>();
                _spawnedItems.Add(go);
                yield return new WaitForSeconds(_delayBetweenItems);
            }
        }

        yield return null;
    }

    public void PrepareToLeave(float totalScannedPrice)
    {
        //TODO : Handle client satisfaction based on the totalScannedPrice
        float supposedTotal = _spawnedItems.Sum(item => item.So.ItemPrice);
        float satisfaction = totalScannedPrice / supposedTotal;

        foreach (FurnitureBehavior item in _spawnedItems)
        {
            Destroy(item);
        }
    }

    private List<FurnitureBehavior> _spawnedItems;

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