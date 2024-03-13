using UnityEngine;

namespace Game.Code.Scripts.UI
{
    [CreateAssetMenu(fileName = "NewEvent", menuName = "Events", order = 0)]
    public class EventSO : ScriptableObject
    {
        public delegate void EventAction(ScriptableObject so);
        public event EventAction OnEvent;

        public void RaiseEvent(ScriptableObject so)
        {
            OnEvent?.Invoke(so);
        }
    }
}