using Game.Code.Scripts;
using Game.Code.Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private ShelfItemEventSO _onRaycastEnter;
    [SerializeField] private ShelfItemEventSO _onRaycastExit;
    [SerializeField] private ShelfItemEventSO _onRaycastGrab;
    
    [Header("Item Settings")]
    [SerializeField, ReadOnly] private ItemSo _itemInfo;
    [SerializeField, ReadOnly] private int _maxItemNumber;
    [SerializeField, ReadOnly] private int _currentItemNumber;
    
    private void Awake()
    {
        _maxItemNumber = Random.Range(3, 10);
        _currentItemNumber = Random.Range(1, _maxItemNumber);
    }
    
    public void OnRaycastEnter()
    {
        _onRaycastEnter.RaiseEvent(new ShelfItem()
            { ID = GetInstanceID(), ItemNumber = _currentItemNumber, MaxItemNumber = _maxItemNumber, ItemSo = _itemInfo });
    }
    
    public void OnRaycastGrab()
    {
        if (_currentItemNumber <= 0)
            return;
        
        _currentItemNumber--;
        
        _onRaycastGrab.RaiseEvent(new ShelfItem()
            { ID = GetInstanceID(), ItemNumber = _currentItemNumber, MaxItemNumber = _maxItemNumber, ItemSo = _itemInfo });
    }

    public void OnRaycastExit()
    {
        _onRaycastExit.RaiseEvent(new ShelfItem()
            { ID = GetInstanceID(), ItemNumber = _currentItemNumber, MaxItemNumber = _maxItemNumber, ItemSo = _itemInfo });
    }
}