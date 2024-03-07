using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class ItemRegistry : SerializedMonoBehaviour
{
    #region Singleton

    public static ItemRegistry Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple App instances!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    #endregion
    
    public ItemSo RandomItem => _items[Random.Range(0, _items.Count)];
    
    public int ItemsCount => _items.Count;
    
    public List<ItemSo> Items => _items;
    
    [SerializeField] private List<ItemSo> _items;
}