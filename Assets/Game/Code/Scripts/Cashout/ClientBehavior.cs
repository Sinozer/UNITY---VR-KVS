using System.Collections;
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
                Furniture furniture = Instantiate(item.Key.Prefab, spawnPoint, Quaternion.identity)
                    .GetComponent<Furniture>();
                furniture.Initialize(item.Key);
                _spawnedItems.Add(furniture);
                yield return new WaitForSeconds(_delayBetweenItems);
            }
        }

        yield return null;
    }

    public void PrepareToLeave(float totalScannedPrice)
    {
        //TODO : Handle client satisfaction based on the totalScannedPrice
        float supposedTotal = _spawnedItems.Sum(item => item.ProductSo.ItemPrice);
        float satisfaction = totalScannedPrice / supposedTotal;

        foreach (Furniture item in _spawnedItems)
        {
            Destroy(item.gameObject);
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
    }
}