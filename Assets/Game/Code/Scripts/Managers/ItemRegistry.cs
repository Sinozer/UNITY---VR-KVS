using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class ItemRegistry : SerializedMonoBehaviour
{
    public ItemSo RandomItem => _items[Random.Range(0, _items.Count)];
    
    public int ItemsCount => _items.Count;
    
    public List<ItemSo> Items => _items;
    
    [SerializeField] private List<ItemSo> _items;
}