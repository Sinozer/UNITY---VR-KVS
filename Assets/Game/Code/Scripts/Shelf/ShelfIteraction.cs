using System;
using Game.Code.Scripts;
using Game.Code.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

public class ShelfIteraction : MonoBehaviour
{
    [SerializeField] private ShelfItemEventSO _onRaycastEnter;
    [SerializeField] private ShelfItemEventSO _onRaycastExit;
    [SerializeField] private ShelfItemEventSO _onRaycastGrab;

    [SerializeField] private ProductShelfUI _menu;

    private int _currentItemId;

    private void OnEnable()
    {
        _onRaycastEnter.OnEvent += Show;
        _onRaycastExit.OnEvent += Hide;
        _onRaycastGrab.OnEvent += SpawnProduct;
    }

    private void OnDisable()
    {
        _onRaycastEnter.OnEvent -= Show;
        _onRaycastExit.OnEvent -= Hide;
        _onRaycastGrab.OnEvent -= SpawnProduct;
    }

    public void Show(ShelfItem item)
    {
        _currentItemId = item.ID;

        _menu.NameText = item.ItemSo.ItemName;
        _menu.ValueText = item.ItemNumber + " / " + item.MaxItemNumber;
        _menu.PriceText = item.ItemSo.ItemPrice.ToString();
        _menu.gameObject.SetActive(true);
    }

    public void Hide(ShelfItem item)
    {
        if (item.ID != _currentItemId)
            return;

        _menu.gameObject.SetActive(false);
    }

    public void SpawnProduct(ShelfItem item)
    {
        if (item.ID != _currentItemId)
            return;

        Furniture product = Instantiate(item.ItemSo.Prefab, transform.position, Quaternion.identity)
            .GetComponent<Furniture>();
        
        product.Initialize(item.ItemSo);
    }
}