using UnityEngine;

namespace Game.Code.Scripts.UI
{
    [CreateAssetMenu(fileName = "NewShelfItemEvent", menuName = "Event/ShelfItem", order = 0)]
    public class ShelfItemEventSO : ScriptableObject
    {
        public delegate void EventAction(ShelfItem shelfItem);
        public event EventAction OnEvent;
        
        public void RaiseEvent(ShelfItem shelfItem)
        {
            OnEvent?.Invoke(shelfItem);
        }
    }
}