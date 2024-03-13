using UnityEngine;

namespace Game.Code.Scripts.UI
{
    [CreateAssetMenu(fileName = "NewItemSoEvent", menuName = "Event/ItemSo", order = 0)]
    public class ItemEventSO : ScriptableObject
    {
        public delegate void EventAction(ItemSo so);
        public event EventAction OnEvent;

        public void RaiseEvent(ItemSo so)
        {
            OnEvent?.Invoke(so);
        }
    }
}